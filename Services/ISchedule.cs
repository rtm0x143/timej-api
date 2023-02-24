using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Services
{
    public interface ISchedule
    {
        Task<LessonDto[]> EditLessons(Guid replicaId, LessonCreation lesson);
        Task DeleteLessons(Guid replicaId);
        Task<LessonDto> EditSingle(Guid id, LessonCreation lesson);
        Task DeleteSingle(Guid id);

        Task<LessonDto[]> CreateLessons(LessonCreation lesson, DateOnly? beginDate,
                DateOnly? endDate);
        Task<LessonDto[]> Get(
            DateOnly beginDate,
            DateOnly endDate,
            uint? groupNumber,
            Guid? teacherId,
            Auditory? auditory,
            bool isOnline);
    }
}
