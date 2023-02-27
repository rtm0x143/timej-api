using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TimejApi.Data.Entities
{
    public record Auditory
    {
        public Guid Id { get; set; }
        public uint AuditoryNumber { get; set; }
        public string? Title { get; set; }
        [JsonIgnore]
        public Building Building { get; set; }
    }
}
