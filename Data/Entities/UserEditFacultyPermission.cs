using Microsoft.EntityFrameworkCore;
using static TimejApi.Data.Entities.User;

namespace TimejApi.Data.Entities
{
    [PrimaryKey(nameof(EditorId), nameof(AllowedFacultyId))]
    public class UserEditFacultyPermission
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public User Editor { get; set; }
        public Guid EditorId { get; set; }
        public Faculty AllowedFaculty { get; set; }
        public Guid AllowedFacultyId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
