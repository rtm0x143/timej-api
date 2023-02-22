using System.ComponentModel.DataAnnotations;

namespace TimejApi.Data.Models
{

    public record Group(int groupNumber)
    {
        public Guid Id { get; set; }
        public Faculty Faculty { get; set; }
        public uint GroupNumber { get; set; }
        public ICollection<LessonGroup>? Lessons { get; set; }

    }
}
