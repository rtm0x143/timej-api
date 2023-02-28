using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public string? MiddleName { get; set; }
    public Gender Gender { get; set; }
}

public record UserEditDto(string Email);
public record ChangePasswordDto(string OldPassword, string NewPassword);

public record UserPublicDto : UserPublicData
{
    public Guid Id{ get; set; }
    public UserPublicDto(Guid id) => Id = id;
    public UserPublicDto() { }
}

public record UserData : UserPublicData, IValidatableObject
{
    public User.Role[] Roles { get; set; }
    public GroupDto? Group { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Group != null && !Roles.Contains(User.Role.STUDENT))
        {
            yield return new ValidationResult("User related to some Group should also have \"STUDENT\" role", new[] { nameof(Roles), nameof(Group) });
        }
        else if (Roles.Contains(User.Role.STUDENT) && Group == null)
        {
            yield return new ValidationResult("User with have \"STUDENT\" role should be related to some Group", new[] { nameof(Roles), nameof(Group) });
        }
    }
}

public record UserRegister : UserData
{
    [PasswordPropertyText]
    [StringLength(20, MinimumLength = 6)]
    public string Password { get; set; }
}

public record UserDto : UserData
{
    public Guid Id { get; set; }
    // Added for compatability 
    public UserDto(Guid id) { Id = id; }
    public UserDto() { }
}

public record AuthResult(string AccessToken, string RefreshToken);

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
