using Xunit.Abstractions;

namespace TimejApi.Tests;
using Services;

public class PasswordHasherTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly PasswordHasher _hasher = new PasswordHasher();

    private const string DefaultPassword = "some_strength_password_123";
    private const string DefaultPasswordWithCapital = "Some_strength_password_123";
    private const string HashedDefaultPassword = "nyTjPpzLJK5zmpMunJS1HKmUGEp3MVMJoYy1pY5Yr9fCgTES";
    
    private const string SymbolicPassword1 = "!$#@$*$@#$#$@*)@$&%$)(@#($)_$&@#$";
    private const string SymbolicPassword2 = "$#@$*$@#$#$@*)@$&%$)(@#($)_$&@#$";

    public PasswordHasherTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory] 
    [InlineData(DefaultPassword)] [InlineData(HashedDefaultPassword)] [InlineData(SymbolicPassword1)] [InlineData("")]
    public void HashPassword_PasswordAndHashNotEquals(string password)
    {
        Assert.NotEqual(password, _hasher.HashPassword(password));
    }
    
    [Theory] 
    [InlineData(DefaultPassword)] [InlineData(HashedDefaultPassword)] [InlineData(SymbolicPassword1)] [InlineData("")]
    public void HashPassword_HashesForSamePasswordsNotEquals(string password)
    {
        var hash1 = _hasher.HashPassword(password);
        var hash2 = _hasher.HashPassword(password);
        Assert.NotEqual(hash1, hash2);
    }
    
    [Theory]
    [InlineData(DefaultPassword)] [InlineData(HashedDefaultPassword)] [InlineData(SymbolicPassword1)] [InlineData("")]
    public void HashPassword_ZeroHashSizeFails(string password)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _hasher.HashPassword(password, 20, 0));
    }
    
    [Theory]
    [InlineData(DefaultPassword)] [InlineData(HashedDefaultPassword)] [InlineData(SymbolicPassword1)] [InlineData("")]
    public void HashPassword_NegativeHashSizeFails(string password)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _hasher.HashPassword(password, 20, -20));
    }
    
    [Theory]
    [InlineData(DefaultPassword)] [InlineData(HashedDefaultPassword)] [InlineData(SymbolicPassword1)] [InlineData("")]
    public void HashPassword_ZeroSaltSizeFails(string password)
    {
        Assert.Throws<Exception>(() => _hasher.HashPassword(password, 0, 20));
    }

    [Theory]
    [InlineData(DefaultPassword)] [InlineData(HashedDefaultPassword)] [InlineData(SymbolicPassword1)] [InlineData("")]
    public void VerifyPassword_CorrectPasswordVerifies(string password)
    {
        var hash = _hasher.HashPassword(password);
        Assert.True(_hasher.VerifyPassword(password, hash));
    }
    
    [Theory]
    [InlineData(HashedDefaultPassword, DefaultPassword)] [InlineData(HashedDefaultPassword, "")] [InlineData("", " ")]
    [InlineData(DefaultPassword, DefaultPasswordWithCapital)] [InlineData(SymbolicPassword1, SymbolicPassword2)]
    public void VerifyPassword_IncorrectPasswordDeclines(string password, string fakePassword)
    {
        var hash = _hasher.HashPassword(password);
        Assert.False(_hasher.VerifyPassword(fakePassword, hash));
    }
}
