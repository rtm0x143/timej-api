using TimejApi.Data.Entities;

namespace TimejApi.Data.Dtos;

public record GroupDto(Guid Id, uint GroupNumber);
public record LessonGroupDto(Guid Id, uint? SubGroupNumber, uint GroupNumber) : GroupDto(Id, GroupNumber);

public record Teacher(Guid Id, string Fullname);

public record LessonDto(Guid Id, Guid replicaId, DateOnly Date, uint LessonNumber, string LessonType,
    string Subject, LessonGroupDto[] Groups, Teacher Teacher, Auditory Auditory);

public record ScheduleDay(DateOnly Date, LessonDto[] Lessons);

public record LessonCreation(DateOnly Date, uint LessonNumber, Guid LessonId,
    Guid SubjectId, LessonGroupDto[] Groups, Guid TeacherId);

public record BuildingCreation(string Title);
public record AuditoryCreation(uint Number);
public record FacultyCreation(string Name);
public record SubjectCreation(uint Name);
