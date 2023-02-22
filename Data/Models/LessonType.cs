namespace TimejApi.Data.Models
{
    public record LessonType
    {
        public LessonType(string name)
        {
            Name = name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
