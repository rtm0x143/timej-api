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

public record LessonDto(Guid Id, Guid ReplicaId, DateOnly Date, LessonNumber LessonNumber, LessonType LessonType,
    Subject Subject, LessonGroupDto[] Groups, Teacher Teacher, Auditory Auditory);

public record ScheduleDay(DateOnly Date, LessonDto[] Lessons);

public record LessonGroupCreation(Guid GroupId, uint? SubGroupNumber);
public record LessonCreation(DateOnly Date, LessonNumber LessonNumber, Guid LessonTypeId,
    Guid SubjectId, LessonGroupCreation[] AttendingGroups, Guid TeacherId, Guid? AuditoryId);

public record BuildingCreation(uint Number, string Title, string? Address);
public record AuditoryCreation(Guid BuildingId, uint AuditoryNumber, string? Title);
public record FacultyCreation(string Name);
public record SubjectCreation(string Name);
