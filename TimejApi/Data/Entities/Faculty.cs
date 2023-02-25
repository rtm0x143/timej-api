using System.Text.Json.Serialization;

namespace TimejApi.Data.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public record Faculty
    {
        public Faculty() { }
        public Faculty(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<User>? Editors { get; set; }
        [JsonIgnore]
        public ICollection<Group> Groups { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
