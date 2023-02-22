namespace TimejApi.Data.Models
{
    public record Faculty
    {
        public Faculty(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserEditFacultyPermission>? Editors { get; set; }

    }
}
