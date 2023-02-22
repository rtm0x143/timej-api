namespace TimejApi.Data.Entities
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
