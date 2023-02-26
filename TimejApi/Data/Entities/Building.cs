using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TimejApi.Data.Entities
{
    public record Building
    {
        public Guid Id { get; set; }
        public uint? Number { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
    }
}
