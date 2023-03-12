using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;
using TimejApi.Helpers.Types;

namespace TimejApi.Services
{
    public interface ISchedule
    {
        /// <exception cref="KeyNotFoundException"></exception>
        Task<Result<Guid[], Exception>> GetAttendingGroupsByReplica(Guid replicaId);
        /// <exception cref="KeyNotFoundException"></exception>
        Task<Result<Guid[], Exception>> GetAttendingGroups(Guid id);
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentException">When had collision</exception>
        Task<Result<Lesson, Exception>> EditLessons(Guid replicaId, LessonEdit lesson);
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentException">When had collision</exception>
        Task<Result<Lesson, Exception>> EditSingle(Guid id, LessonEdit lesson);
        Task DeleteSingle(Guid id);
        Task DeleteLessons(Guid replicaId);

        Task<LessonDto> CreateLessons(LessonCreation lesson, DateOnly? beginDate,
                DateOnly? endDate, uint repeatInterval=1);
        Task<ScheduleDay[]> Get(
            DateOnly beginDate,
            DateOnly endDate,
            Guid? groupNumber,
            Guid? teacherId,
            Guid? auditoryId,
            bool isOnline = false);
    }
}
