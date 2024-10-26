using NUnit.Framework;
using API.Modules.AccountsModule.Share;

namespace IntegrationTests.PasswordHasherTests;

public class VerifyPasswordTest
{
    private IPasswordHasher passwordHasher;

    [OneTimeSetUp]
    public void SetUp()
    {
        passwordHasher = new PasswordHasher();
    }

    [Test]
    public void VerifyPassword_WhenPasswordMatches_ReturnsTrue()
    {
        var password = "mySecurePassword123";
        var hashedPassword = passwordHasher.Hash(password);

        var result = passwordHasher.VerifyPassword(password, hashedPassword);

        Assert.IsTrue(result, "пароли совпадают.");
    }

    [Test]
    public void VerifyPassword_WhenPasswordDoesNotMatch_ReturnsFalse()
    {
        var password = "mySecurePassword123";
        var hashedPassword = passwordHasher.Hash(password);
        var wrongPassword = "wrongPassword";

        var result = passwordHasher.VerifyPassword(wrongPassword, hashedPassword);

        Assert.IsFalse(result, "пароли не совпадают.");
    }
}