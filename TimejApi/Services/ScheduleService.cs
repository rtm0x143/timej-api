using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimejApi.Data;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;
using TimejApi.Helpers.Types;
using EntityFramework.Exceptions.Common;

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

        public async Task<Result<Lesson, Exception>> EditLessons(Guid replicaId, LessonEdit lesson)
        {
            var lessons = await _dbContext.Lessons.Where(x => x.ReplicaId == replicaId)
                .Include(l => l.AttendingGroups)
                .ToListAsync();

            if (lessons.IsNullOrEmpty()) return new(new KeyNotFoundException(nameof(replicaId)));

            foreach (var item in lessons)
            {
                lesson.Adapt(item);
                itemDate = item.Date.AddDays(lesson.ShiftDays);
                if (await CheckCollisions(item))
                    return new(new ArgumentException($"The lesson tried to be moved to {item.Date} in {item.LessonNumber} timeslot has collisions"));
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (ReferenceConstraintException ex)
            {
                return new(new ArgumentException($"Data in new lesson contains incorrect relations", ex));
            }
            catch (Exception ex) { return new(ex); }

            return new(await new LessonQuerry(_dbContext.Lessons)
                .GetEnriched()
                .FirstAsync(l => l.ReplicaId == replicaId));
        }

        public async Task<ScheduleDay[]> Get(DateOnly beginDate, DateOnly endDate, Guid? groupNumber, Guid? teacherId, Guid? auditoryId, bool isOnline)
        {
            var query = new LessonQuerry(_dbContext.Lessons);
            var timetable = await query.SpecifyDate(beginDate, endDate).SpecifyGroup(groupNumber).SpecifyPlace(auditoryId, isOnline)
                .SpecifyTeacher(teacherId).GetEnriched().OrderBy(x => x.LessonNumber).GroupBy(x => x.Date).ToDictionaryAsync(x => x.Key, x => x.ToArray());
            var result = new List<ScheduleDay>();
            foreach (var (day, schedule) in timetable)
            {
                var dailySchedule = new LessonDto[Enum.GetNames(typeof(LessonNumber)).Length];
                foreach (var lesson in schedule)
                {
                    var slot = (int)lesson.LessonNumber;
                    dailySchedule[slot] = lesson.Adapt<LessonDto>();
                }
                result.Add(new ScheduleDay(day, dailySchedule));
            }
            return result.ToArray();
        }

        /// <summary>
        /// <para>Checks if lesson has collisions when inserting into database</para>
        /// i.e. the collision happens when at least one of teacher, auditory or any of groups already has lesson at the same time
        /// </summary>
        /// <returns><c>true</c> if collision happens, <c>false</c> otherwise</returns>
        private async Task<bool> CheckCollisions(Lesson lesson)
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

        public async Task<Result<Lesson, Exception>> EditSingle(Guid id, LessonEdit lessonEdit)
        {
            var lesson = await new LessonQuerry(_dbContext.Lessons)
                .GetEnriched()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null) return new(new KeyNotFoundException(nameof(id)));

            lessonEdit.Adapt(lesson);
            lesson.ReplicaId = Guid.NewGuid();
            lesson.Date.AddDays(lessonEdit.ShiftDays);

            if (await CheckCollisions(lesson))
                return new(new ArgumentException($"The lesson tried to be moved to {lesson.Date} in {lesson.LessonNumber} timeslot has collisions"));

            try
            {
                await _dbContext.SaveChangesAsync();
                return new(lesson);
            }
            catch (ReferenceConstraintException e)
            {
                _logger.LogInformation($"Error updating lesson with ID {id}. The exception was caused by {e.Entries[0].Entity.GetType().Name}");
                return new(new ArgumentException($"Data in new lesson contains incorrect relations", e));
            }
            catch (Exception e) { return new(e); }
        }

        public async Task DeleteSingle(Guid id)
        {
            var result = await _dbContext.Lessons.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<LessonDto> CreateLessons(LessonCreation lesson, DateOnly? beginDate, DateOnly? endDate, uint repeatInterval)
        {

            if (beginDate is null && endDate is null || repeatInterval == 0)
            {
                return await CreateSingle(lesson);
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
                var newLesson = lesson.Adapt<Lesson>();
                newLesson.Date = day;
                var isCollided = await CheckCollisions(newLesson);
                if (isCollided)
                {
                    throw new ArgumentException($"The lesson tried to add on {lesson.Date} in {lesson.LessonNumber} timeslot has collisions");
                }
                return newLesson;
            }

            var step = DEFAULT_INTERVAL_DAYS * ((int)repeatInterval);
            var originalLesson = await ProcessLesson(lesson.Date);
            var lessonsRange = new List<Lesson>();
            for (DateOnly day = lesson.Date.AddDays(-step); day >= beginDate; day = day.AddDays(-step))
            {
                lessonsRange.Add(await ProcessLesson(day));
            }
            for (DateOnly day = lesson.Date.AddDays(step); day <= endDate; day = day.AddDays(step))
            {
                lessonsRange.Add(await ProcessLesson(day));
            }
            lessonsRange.Add(originalLesson);
            var replicaId = Guid.NewGuid();
            await _dbContext.Lessons.AddRangeAsync(lessonsRange.Select(x => { x.ReplicaId = replicaId; return x; }));
            await _dbContext.SaveChangesAsync();

            var lessonSample = await (new LessonQuerry(_dbContext.Lessons)).GetEnriched().FirstAsync(x => x.Id == originalLesson.Id);
            return lessonSample.Adapt<LessonDto>();
        }

        private async Task<LessonDto> CreateSingle(LessonCreation lesson)
        {
            var _lesson = lesson.Adapt<Lesson>();
            _lesson.ReplicaId = Guid.NewGuid();
            var isCollided = await CheckCollisions(_lesson);
            if (isCollided)
            {
                throw new ArgumentException($"The lesson tried to be added on {lesson.Date} in {lesson.LessonNumber} timeslot has collisions");
            }
            await _dbContext.Lessons.AddAsync(_lesson);
            await _dbContext.SaveChangesAsync();
            return _lesson.Adapt<LessonDto>();
        }

        public async Task<Result<Guid[], Exception>> GetAttendingGroupsByReplica(Guid replicaId)
        {
            var lesson = await _dbContext.Lessons.Include(x => x.AttendingGroups)
                .FirstOrDefaultAsync(x => x.ReplicaId == replicaId);

            if (lesson == null) return new(new KeyNotFoundException(nameof(replicaId)));
            return new(lesson.AttendingGroups.Select(x => x.GroupId).ToArray());
        }

        public async Task<Result<Guid[], Exception>> GetAttendingGroups(Guid id)
        {
            var lesson = await _dbContext.Lessons.Include(x => x.AttendingGroups)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (lesson == null) return new(new KeyNotFoundException(nameof(id)));
            return new(lesson.AttendingGroups.Select(x => x.GroupId).ToArray());
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

        public LessonQuerry SpecifyTimeSlot(LessonNumber timeSlot)
        {
            _lessons = _lessons.Where(x => x.LessonNumber == timeSlot);
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
