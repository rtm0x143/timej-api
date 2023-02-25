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
    [Index(nameof(Date), nameof(Number), nameof(TeacherId), IsUnique = true)]
    [Index(nameof(Date), nameof(Number), nameof(AuditoryNumber), nameof(AuditoryBuilding), IsUnique = true)]
    [Index(nameof(ReplicaId))]
    public record Lesson
    {

        public Guid Id { get; set; }
        public Guid ReplicaId { get; set; }
        public DateOnly Date { get; set; }
        public LessonNumber Number { get; set; }
        public LessonType LessonType { get; set; }
        public Subject Subject { get; set; }
        public User Teacher { get; set; }
        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        // If audotory is null then lesson is Online
        [ForeignKey($"{nameof(AuditoryBuilding)},{nameof(AuditoryNumber)}")]
        public Auditory? Auditory { get; set; }
        public uint AuditoryNumber { get; set; }
        public uint AuditoryBuilding { get; set; }
        public ICollection<LessonGroup> AttendingGroups { get; set; }

    }
}
