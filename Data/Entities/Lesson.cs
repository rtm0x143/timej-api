namespace TimejApi.Data.Entities
{
    public enum LessonNumber
    {
        //Ordering of the lesson in a schedule
        UNSPECIFIED,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        Seventh
    }
    public record Lesson
    {

        public Guid Id { get; set; }
        public Guid replicaId { get; set; }
        public DateOnly Date { get; set; }
        public LessonNumber Number { get; set; }
        public LessonType LessonType { get; set; }
        public Subject Subject { get; set; }
        public User Teacher { get; set; }
        // If audotory is null then lesson is Online
        public Auditory? Auditory { get; set; }
        public ICollection<LessonGroup> AttendingGroups { get; set; }

    }
}
