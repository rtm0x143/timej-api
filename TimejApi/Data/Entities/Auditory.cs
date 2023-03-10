using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TimejApi.Data.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Index(nameof(AuditoryNumber), nameof(BuildingId), IsUnique = true)]
    public record Auditory
    {
        public Guid Id { get; set; }
        public uint AuditoryNumber { get; set; }
        public string? Title { get; set; }

        [ForeignKey(nameof(Building))]
        public Guid BuildingId { get; set; }
        [JsonIgnore]
        public Building Building { get; set; }
        public string? Description { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
