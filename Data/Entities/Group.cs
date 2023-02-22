using System.ComponentModel.DataAnnotations;

namespace TimejApi.Data.Entities
{

    public record Group(int groupNumber)
    {
        public Guid Id { get; set; }
        public Faculty Faculty { get; set; }
        public uint GroupNumber { get; set; }
        public ICollection<LessonGroup>? Lessons { get; set; }

    }
}
