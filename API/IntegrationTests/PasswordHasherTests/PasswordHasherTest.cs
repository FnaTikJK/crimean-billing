using API.Modules.AccountsModule.Share;
using FluentAssertions;
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

        hashedPassword.Should().NotBeNull();
        hashedPassword.Should().NotBeEmpty();
    }

    [Test]
    public void Hash_WhenCalledMultipleTimes_ReturnsDifferentHashes()
    {
        var password = "mySecurePassword123";

        var hash1 = passwordHasher.Hash(password);
        var hash2 = passwordHasher.Hash(password);

        hash1.Should().NotBe(hash2, "Хэш с одинаковым паролем не должен совпадать.");
    }

    [Test]
    public void Hash_ThrowsArgumentNullException_WhenPasswordIsNull()
    {
        Action act = () => passwordHasher.Hash("");

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Hash_WithKnownPassword_ReturnsExpectedLengthHash()
    {
        var password = "testPassword123";

        var hash = passwordHasher.Hash(password);

        hash.Length.Should().Be(48);
    }
}