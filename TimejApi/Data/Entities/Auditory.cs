using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TimejApi.Data.Entities
{
    [PrimaryKey(nameof(AuditoryNumber), nameof(BuildingNumber))]
    public record Auditory
    {
        public uint AuditoryNumber { get; set; }
        [ForeignKey(nameof(Building))]
        public uint BuildingNumber { get; set; }
        [JsonIgnore]
        public Building Building { get; set; }
    }
}
