using System.ComponentModel.DataAnnotations;

namespace TimejApi.Data.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public record Group
    {
        public Guid Id { get; set; }
        public Faculty Faculty { get; set; }
        public uint GroupNumber { get; set; }
        public ICollection<LessonGroup>? Lessons { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
