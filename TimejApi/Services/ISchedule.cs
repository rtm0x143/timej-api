using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Services
{
    public interface ISchedule
    {
        Task<Guid[]> GetAttendingGroupsByReplica(Guid replicaId);
        Task<Guid[]> GetAttendingGroups(Guid id);
        Task<LessonDto[]> EditLessons(Guid replicaId, LessonEdit lesson);
        Task DeleteLessons(Guid replicaId);
        Task<LessonDto> EditSingle(Guid id, LessonEdit lesson);
        Task DeleteSingle(Guid id);

        Task<LessonDto[]> CreateLessons(LessonCreation lesson, DateOnly? beginDate,
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
