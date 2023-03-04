using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TimejApi.Data.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Index(nameof(FacultyId), nameof(GroupNumber), IsUnique = true)]
    public record Group
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(Faculty))]
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public uint GroupNumber { get; set; }
        [JsonIgnore]
        public ICollection<LessonGroup>? Lessons { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
