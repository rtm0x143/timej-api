using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TimejApi.Data.Models
{
    [PrimaryKey(nameof(Number))]
    public record Building
    {
        public uint Number { get; set; }
        public string? Title { get; set; }
    }
}
