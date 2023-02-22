using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimejApi.Data.Models
{
    [PrimaryKey(nameof(Role),nameof(UserId))]
    public class UserRole
    {
        public User.Role Role { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public User User { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
