namespace TimejApi.Data.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public record Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Subject() { }
        public Subject(string name)
        {
            Name = name;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
