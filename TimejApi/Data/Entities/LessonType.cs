namespace TimejApi.Data.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public record LessonType
    {
  
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
