using TimejApi.Data.Entities;
using TimejApi.Services;

namespace TimejApi.Tests;

public class Users
{
    public readonly User RegisteredUser;
    public readonly UserLogin RegisteredUserLogin;
    public readonly UserLogin UnregisteredUserLogin;

    public Users(PasswordHasher _hasher)
    {
        const string email = "correct-email@example.com";
        const string pass = "some_strength_password_123";
        var hash = _hasher.HashPassword(pass);

        RegisteredUserLogin = new UserLogin
        {
            Email = email,
            Password = pass
        };
        
        
        RegisteredUser = new User()
        {
            Email = email,
            PasswordHash = hash,
            Name = "Test",
            Surname = "Testing"
        };
        
        
        UnregisteredUserLogin = new UserLogin
        {
            Email = "unregister-email@example.com",
            Password = pass
        };
    }
}