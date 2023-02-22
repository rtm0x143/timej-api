namespace TimejApi.Data.Dtos;

public record LessonGroup(uint? SubGroupNumber, uint GroupNumber);
public record Teacher(Guid Id, string Fullname);

public record LessonDto(Guid Id, DateOnly Date, uint LessonNumber, string LessonType,
    string Subject, LessonGroup[] Groups, Teacher Teacher);

public record ScheduleDay(DateOnly Date, LessonDto[] Lessons);

public record LessonCreation(DateOnly Date, uint LessonNumber, Guid LessonId,
    Guid SubjectId, LessonGroup[] Groups, Guid TeacherId);

public record BuildingCreation(string Title);
public record AuditoryCreation(uint Number);
public record FacultyCreation(uint Name);
public record SubjectCreation(uint Name);
