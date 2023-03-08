using TimejApi.Data.Entities;

namespace TimejApi.Data.Dtos;

public record GroupDto(Guid Id, uint GroupNumber)
{
    public GroupDto() : this(default, default) { }
}

public record LessonGroupDto(Guid Id, uint? SubGroupNumber, uint GroupNumber) : GroupDto(Id, GroupNumber)
{
    public LessonGroupDto() : this(default, default, default) { }
}

public record GroupCreaton(uint GroupNumber);

public record Teacher(Guid Id, string Fullname);

public record LessonDto(Guid Id, Guid replicaId, DateOnly Date, uint LessonNumber, string LessonType,
    string Subject, LessonGroupDto[] Groups, Teacher Teacher, Auditory Auditory);

public record ScheduleDay(DateOnly Date, LessonDto[] Lessons);

public record LessonCreation(DateOnly Date, uint LessonNumber, Guid LessonTypeId,
    Guid SubjectId, Guid[] GroupsIds, Guid TeacherId);

public record BuildingCreation(uint Number, string Title, string? Address);
public record AuditoryCreation(Guid BuildingId, uint AuditoryNumber, string? Title);
public record FacultyCreation(string Name);
public record SubjectCreation(string Name);
