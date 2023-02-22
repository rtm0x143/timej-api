using System.Text.Json.Serialization;

namespace TimejApi.Data.Entities
{
    public record Faculty
    {
        public Faculty(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<UserEditFacultyPermission>? Editors { get; set; }
    }
}
