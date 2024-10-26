using API.Modules.AccountsModule.Share;
using NUnit.Framework;

namespace IntegrationTests.PasswordHasherTests;

public class PasswordHasherTest
{
    private IPasswordHasher passwordHasher;

    [SetUp]
    public void Setup()
    {
        passwordHasher = new PasswordHasher();
    }

    [Test]
    public void Hash_WhenCalled_ReturnsHashedPassword()
    {
        var password = "mySecurePassword123";

        var hashedPassword = passwordHasher.Hash(password);

        Assert.IsNotNull(hashedPassword);
        Assert.IsNotEmpty(hashedPassword);
        Console.WriteLine(hashedPassword);
    }

    [Test]
    public void Hash_WhenCalledMultipleTimes_ReturnsDifferentHashes()
    {
        var password = "mySecurePassword123";

        var hash1 = passwordHasher.Hash(password);
        var hash2 = passwordHasher.Hash(password);

        Assert.AreNotEqual(hash1, hash2, "Хэш с одинаковым паролем не должен совпадать.");
    }

    [Test]
    public void Hash_ThrowsArgumentNullException_WhenPasswordIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => passwordHasher.Hash(null));
    }

    [Test]
    public void Hash_WithKnownPassword_ReturnsExpectedLengthHash()
    {
        var password = "testPassword123";

        var hash = passwordHasher.Hash(password);

        Assert.AreEqual(48, hash.Length);
    }
}