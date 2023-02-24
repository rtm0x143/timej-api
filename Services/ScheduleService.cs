using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var lessons = await _dbContext.Lessons.Where(x => x.replicaId == replicaId).ToListAsync();
            foreach (var item in lessons)
            {
                lesson.Adapt(item);
            }
            await _dbContext.SaveChangesAsync();
            return lessons.Select(x => x.Adapt<LessonDto>()).ToArray();
        }

        public async Task<LessonDto[]> Get(DateOnly beginDate, DateOnly endDate, uint? groupNumber, Guid? teacherId, Auditory? auditory, bool isOnline)
        {
            var group = _dbContext.Groups.Where(x => x.groupNumber == groupNumber);
            return await _dbContext.Lessons.Where(x => x.Date >= beginDate && x.Date <= endDate
            && (teacherId == null || x.Teacher.Id == teacherId)
            && (group == null || x.AttendingGroups.Select(y => y.Group).Contains(group as Group))
            && (isOnline ? x.Auditory == null : auditory == null || x.Auditory == auditory)).Select(x => x.Adapt<LessonDto>()).ToArrayAsync();
        }

        public async Task DeleteLessons(Guid replicaId)
        {
            await _dbContext.Lessons.Where(x => x.replicaId == replicaId).ExecuteDeleteAsync();
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
            await _dbContext.Lessons.Where(x => x.Id == id).ExecuteDeleteAsync();
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

            var lessonsRange = new List<Lesson>();
            for (DateOnly day = (DateOnly)beginDate; day <= endDate; day.AddDays(DEFAULT_INTERVAL_DAYS))
            {                
                lessonsRange.Add(lesson.Adapt<Lesson>());
                lesson.Date.AddDays(DEFAULT_INTERVAL_DAYS);
            }
            await _dbContext.Lessons.AddRangeAsync(lessonsRange);
            await _dbContext.SaveChangesAsync();
            return lessonsRange.Select(x => x.Adapt<LessonDto>()).ToArray();
        }

        private async Task<LessonDto> CreateSingle(LessonCreation lesson)
        {
            var _lesson = lesson.Adapt<Lesson>();
            await _dbContext.Lessons.AddAsync(_lesson);
            await _dbContext.SaveChangesAsync();
            return _lesson.Adapt<LessonDto>();
        }
    }
}
