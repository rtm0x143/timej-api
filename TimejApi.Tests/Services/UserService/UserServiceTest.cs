using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Moq;
using TimejApi.Data;
using TimejApi.Data.Entities;
using TimejApi.Services.User;
using Xunit.Abstractions;

namespace TimejApi.Tests;
using Services;

public class UserServiceTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Users _users;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        var options = new DbContextOptionsBuilder<ScheduleDbContext>()
            .UseInMemoryDatabase(databaseName: "ScheduleDatabase")
            .Options;
        
        var db = new ScheduleDbContext(options);
        var hasher = new Mock<PasswordHasher>().Object;
        
        _users = new Users(hasher);
        _userService = new UserService(db, hasher);

        db.Users.Add(_users.RegisteredUser);
        db.SaveChanges();
    }
    
    [Fact]
    public async Task TryLogin_EmptyPasswordDeclines()
    {
        var loginForm = _users.RegisteredUserLogin with { Password = "" };
        var result = await _userService.TryLogin(loginForm);

        Assert.False(result.Ensure(out var user, out var errors));
        Assert.Equal(1, errors.ErrorCount);

        var error = errors.First().Key;
        Assert.Equal(nameof(loginForm.Password), error);
    }
    
    [Fact]
    public async Task TryLogin_WrongPasswordDeclines()
    {
        var loginForm = _users.RegisteredUserLogin with { Password = "Some_strength_password_123" };
        var result = await _userService.TryLogin(loginForm);

        Assert.False(result.Ensure(out var user, out var errors));
        Assert.Equal(1, errors.ErrorCount);

        var error = errors.First().Key;
        Assert.Equal(nameof(loginForm.Password), error);
    }
    
    [Fact]
    public async Task TryLogin_UnregisterUserDeclines()
    {
        var result = await _userService.TryLogin(_users.UnregisteredUserLogin);

        Assert.False(result.Ensure(out var user, out var errors));
        Assert.Equal(1, errors.ErrorCount);

        var error = errors.First().Key;
        Assert.Equal(nameof(_users.UnregisteredUserLogin.Email), error);
    }
    
    [Fact]
    public async Task TryLogin_CorrectUserAccepts()
    {
        var loginForm = _users.RegisteredUserLogin;
        var result = await _userService.TryLogin(loginForm);

        Assert.True(result.Ensure(out var user, out var errors));
        Assert.Equal(loginForm.Email, user.Email);
    }
}
