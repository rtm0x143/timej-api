namespace TimejApi.Data.Models
{
    public record Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Subject(string name)
        {
            Name = name;
        }
    }
}
