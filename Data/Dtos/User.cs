using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimejApi.Data.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UserLogin
{
    [EmailAddress]
    public string Email { get; set; }
    [PasswordPropertyText]
    public string Password { get; set; }
}

public record UserBase
{
    [EmailAddress]
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Middlename { get; set; }
    public Gender Gender { get; set; }
    public User.Role[] Roles { get; set; }
    public int? GroupNumber { get; set; }
}

public record UserRegister : UserBase
{
    [PasswordPropertyText]
    public string Password { get; set; }
}

public record UserDto(Guid Id) : UserBase;

public record AuthResult(string Token);

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.