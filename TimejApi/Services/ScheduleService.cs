using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimejApi.Data;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Services
{
    public class ScheduleService : ISchedule
    {
        private readonly ScheduleDbContext _dbContext;
        private readonly ILogger<ScheduleService> _logger;
        private const int DEFAULT_INTERVAL_DAYS = 7;

        public ScheduleService(ILogger<ScheduleService> logger, ScheduleDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<LessonDto[]> EditLessons(Guid replicaId, LessonCreation lesson)
        {
            var lessons = await _dbContext.Lessons.Where(x => x.ReplicaId == replicaId).ToListAsync();
            foreach (var item in lessons)
            {
                lesson.Adapt(item);
            }
            await _dbContext.SaveChangesAsync();
            return lessons.Select(x => x.Adapt<LessonDto>()).ToArray();
        }

        public async Task<ScheduleDay[]> Get(DateOnly beginDate, DateOnly endDate, Guid? groupNumber, Guid? teacherId, Guid? auditoryId, bool isOnline)
        {

            var query = new LessonQuerry(_dbContext.Lessons);
            var timetable = await query.SpecifyDate(beginDate, endDate).SpecifyGroup(groupNumber).SpecifyPlace(auditoryId, isOnline)
                .SpecifyTeacher(teacherId).GetEnriched().OrderBy(x => x.LessonNumber).GroupBy(x => x.Date).ToDictionaryAsync(x => x.Key, x => x.ToArray());
            var result = new List<ScheduleDay>();
            foreach (var (day, schedule) in timetable)
            {
                //TODO maybe remove UNSPECIFIED in LessonNumber enum, to not perform -1
                var dailySchedule = new LessonDto[Enum.GetNames(typeof(LessonNumber)).Length - 1];
                foreach (var lesson in schedule)
                {
                    var slot = (int)lesson.LessonNumber;
                    dailySchedule[slot] = lesson.Adapt<LessonDto>();
                }
                result.Add(new ScheduleDay(day, dailySchedule));
            }
            return result.ToArray();
        }

        private async Task<bool> CheckCollisions(LessonCreation lesson)
        {
            var isOnline = lesson.AuditoryId is null;
            var query = new LessonQuerry(_dbContext.Lessons);
            var lessonMoment = query.SpecifyDate(lesson.Date, lesson.Date).SpecifyTimeSlot(lesson.LessonNumber);
            var teacherBusy = !(await lessonMoment.SpecifyTeacher(lesson.TeacherId).Get().ToArrayAsync()).IsNullOrEmpty();
            var auditoryBusy = !(isOnline ? null : await lessonMoment.SpecifyPlace(lesson.AuditoryId, false).Get().ToArrayAsync()).IsNullOrEmpty();
            var groupsAnyBusy = !(await lessonMoment.SpecifyGroupsAny(lesson.AttendingGroups.Select(x => x.GroupId).ToArray()).Get().ToArrayAsync()).IsNullOrEmpty();
            _logger.LogInformation($"Collisions for lesson {lesson.Date}, timeslot: {lesson.LessonNumber}\n" +
                $" teacher {lesson.TeacherId} is busy: {teacherBusy} \n" +
                $" auditory {lesson.AuditoryId} is busy: {auditoryBusy}\n" +
                $" some groups are busy: {groupsAnyBusy}\n");
            return teacherBusy && auditoryBusy && groupsAnyBusy;
        }

        public async Task DeleteLessons(Guid replicaId)
        {
            await _dbContext.Lessons.Where(x => x.ReplicaId == replicaId).ExecuteDeleteAsync();
        }

        public async Task<LessonDto> EditSingle(Guid id, LessonCreation lesson)
        {
            var _lesson = await _dbContext.Lessons.FindAsync(id) ?? throw new KeyNotFoundException($"Attempt to edit non-existent lesson with id {id}");
            lesson.Adapt(_lesson);
            await _dbContext.SaveChangesAsync();
            return _lesson.Adapt<LessonDto>();
        }

        public async Task DeleteSingle(Guid id)
        {
            var result = await _dbContext.Lessons.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<LessonDto[]> CreateLessons(LessonCreation lesson, DateOnly? beginDate, DateOnly? endDate)
        {

            if (beginDate is null && endDate is null)
            {
                return new LessonDto[] { await CreateSingle(lesson) };
            }
            else if (beginDate is null || endDate is null)
            {
                throw new ArgumentException("Both or none of beginDate and endDate must be specified");
            }
            if (lesson.Date < beginDate || lesson.Date > endDate)
            {
                throw new ArgumentException("The lesson must be within bounds you want to create it in");
            }


            async Task<Lesson> ProcessLesson(DateOnly day)
            {
                var isCollided = await CheckCollisions(lesson);
                if (isCollided)
                {
                    throw new ArgumentException($"The lesson you tried to add on {lesson.Date} in {lesson.LessonNumber} slot had collisions");
                }
                var newLesson = lesson.Adapt<Lesson>();
                newLesson.Date = day;
                return newLesson;
            }


            var lessonsRange = new List<Lesson>();
            for (DateOnly day = lesson.Date.AddDays(-DEFAULT_INTERVAL_DAYS); day >= beginDate; day = day.AddDays(-DEFAULT_INTERVAL_DAYS))
            {
                lessonsRange.Add(await ProcessLesson(day));
            }
            for (DateOnly day = lesson.Date; day <= endDate; day = day.AddDays(DEFAULT_INTERVAL_DAYS))
            {
                lessonsRange.Add(await ProcessLesson(day));
            }


            await _dbContext.Lessons.AddRangeAsync(lessonsRange.Select(x => { x.ReplicaId = Guid.NewGuid(); return x; }));
            await _dbContext.SaveChangesAsync();
            return lessonsRange.Select(x => x.Adapt<LessonDto>()).ToArray();
        }

        private async Task<LessonDto> CreateSingle(LessonCreation lesson)
        {
            var _lesson = lesson.Adapt<Lesson>();
            _lesson.AttendingGroups = new List<LessonGroup>();
            foreach (var group in lesson.AttendingGroups)
            {
                _lesson.AttendingGroups.Append(group.Adapt<LessonGroup>());
            }
            await _dbContext.Lessons.AddAsync(_lesson);
            await _dbContext.SaveChangesAsync();
            return _lesson.Adapt<LessonDto>();
        }

        private void AdaptGroups(Lesson lesson, LessonCreation lessonCreation)
        {

        }
    }

    public class LessonQuerry
    {
        private IQueryable<Lesson> _lessons;

        public LessonQuerry(IQueryable<Lesson> lessons)
        {
            _lessons = lessons;
        }
        public IQueryable<Lesson> Get()
        {
            return _lessons;
        }

        public IQueryable<Lesson> GetEnriched()
        {
            return _lessons.Include(x => x.Teacher).
                Include(x => x.Auditory).Include(x => x.AttendingGroups).ThenInclude(x => x.Group).
                Include(x => x.Auditory).ThenInclude(x => x.Building).
                Include(x => x.LessonType).Include(x => x.Subject);
        }
        public LessonQuerry SpecifyDate(DateOnly beginDate, DateOnly endDate)
        {
            _lessons = _lessons.Where(x => x.Date >= beginDate && x.Date <= endDate);
            return this;
        }

        public LessonQuerry SpecifyTimeSlot(uint timeSlot)
        {
            LessonNumber slotEnum = (LessonNumber)Enum.ToObject(typeof(LessonNumber), timeSlot);
            _lessons = _lessons.Where(x => x.LessonNumber == slotEnum);
            return this;
        }
        public LessonQuerry SpecifyTeacher(Guid? teacherId)
        {
            if (teacherId is null) return this;
            _lessons = _lessons.Where(x => x.Teacher.Id == teacherId);
            return this;
        }

        public LessonQuerry SpecifyGroup(Guid? groupId)
        {
            if (groupId is null) return this;
            _lessons = _lessons.Where(x => x.AttendingGroups.Select(x => x.GroupId).Contains((Guid)groupId)).Include(x => x.AttendingGroups);
            return this;
        }

        public LessonQuerry SpecifyGroupsAny(Guid[] groupIds)
        {
            _lessons = _lessons.Where(x => x.AttendingGroups.Any(x => groupIds.Contains(x.GroupId)));
            return this;
        }

        public LessonQuerry SpecifyPlace(Guid? auditoryId, bool isOnline)
        {
            if (isOnline)
            {
                _lessons = _lessons.Where(x => x.Auditory == null);
                return this;
            }
            if (auditoryId is null) return this;
            _lessons = _lessons.Where(x => x.AuditoryId == auditoryId);
            return this;
        }
    }
}
