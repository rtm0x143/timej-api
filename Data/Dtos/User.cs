using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimejApi.Data.Entities;

public record UserLogin
{
    [EmailAddress]
    public string Email { get; set; }
    [PasswordPropertyText]
    public string Password { get; set; }
}

public record UserRegister
{
    [EmailAddress]
    public string Email { get; set; }
    [PasswordPropertyText]
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Middlename { get; set; }
    public Gender Gender { get; set; }
    public ICollection<User.Role> Roles { get; set; }
    public int GroupNumber { get; set; }
}