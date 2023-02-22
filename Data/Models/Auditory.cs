using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimejApi.Data.Models
{
    [PrimaryKey(nameof(AuditoryNumber), nameof(BuildingNumber))]
    public record Auditory
    {

        public uint AuditoryNumber { get; set; }
        [ForeignKey(nameof(Building))]
        public uint BuildingNumber { get; set; }
        public Building Building { get; set; }
    }
}
