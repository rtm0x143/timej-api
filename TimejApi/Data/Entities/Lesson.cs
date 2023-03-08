using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimejApi.Data.Entities
{
    public enum LessonNumber
    {
        //Ordering of the lesson in a schedule
        UNSPECIFIED,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        Seventh
    }
    [Index(nameof(Date), nameof(LessonNumber), nameof(TeacherId), IsUnique = true)]
    [Index(nameof(Date), nameof(LessonNumber), nameof(AuditoryId), IsUnique = true)]
    [Index(nameof(ReplicaId))]
    public record Lesson
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Guid Id { get; set; }
        public Guid ReplicaId { get; set; }
        public DateOnly Date { get; set; }
        public LessonNumber LessonNumber { get; set; }
        public LessonType LessonType { get; set; }
        [ForeignKey(nameof(LessonType))]
        public Guid? LessonTypeId { get; set; }
        public Subject Subject { get; set; }
        [ForeignKey(nameof(Subject))]
        public Guid? SubjectId { get; set; }
        public User Teacher { get; set; }
        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        // If audotory is null then lesson is Online
        public Auditory? Auditory { get; set; }
        [ForeignKey(nameof(Auditory))]
        public Guid? AuditoryId { get; set; }

        public ICollection<LessonGroup> AttendingGroups { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}
