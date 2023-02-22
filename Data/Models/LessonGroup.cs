using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimejApi.Data.Models
{
    [PrimaryKey(nameof(LessonId),nameof(GroupId))]
    public record LessonGroup
    {
        public Lesson Lesson { get; set; }
        [ForeignKey(nameof(Lesson))]
        public Guid LessonId { get; set; }
        public Group Group { get; set; }
        [ForeignKey(nameof(Group))]
        public Guid GroupId { get; set; }
        public uint? SubgroupNumber { get; set; }
    }
}
