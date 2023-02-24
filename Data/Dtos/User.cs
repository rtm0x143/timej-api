using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimejApi.Data.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UserLogin
{
    [EmailAddress]
    public string Email { get; set; }
    [PasswordPropertyText]
    [StringLength(20, MinimumLength = 6)]
    public string Password { get; set; }
}

public record UserPublicData
{
    [EmailAddress]
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string MiddleName { get; set; }
    public Gender Gender { get; set; }
}

public record UserPublicDto(Guid Id) : UserPublicData;

public record UserData : UserPublicData
{
    public User.Role[] Roles { get; set; }
    public uint? GroupNumber { get; set; }
}


public record UserRegister : UserData
{
    [PasswordPropertyText]
    [StringLength(20, MinimumLength = 6)]
    public string Password { get; set; }
}

public record UserDto(Guid Id) : UserData;

public record AuthResult(string Token, string RefreshToken);

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
