using AdminGonbadTala.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace AdminGonbadTala.Tests;

public class KhademPasswordServiceTests
{
    private readonly KhademPasswordService _service =
        new(new PasswordHasher<Khadem>());

    [Fact]
    public void SetPassword_StoresHashThatVerifiesWithTheSamePassword()
    {
        var khadem = new Khadem();

        _service.SetPassword(khadem, "Correct-Horse-99");

        Assert.NotEqual("Correct-Horse-99", khadem.PasswordHash);
        Assert.True(_service.VerifyAndUpgradeIfNeeded(khadem, "Correct-Horse-99"));
    }

    [Fact]
    public void VerifyAndUpgradeIfNeeded_RejectsAnIncorrectPassword()
    {
        var khadem = new Khadem();
        _service.SetPassword(khadem, "Correct-Horse-99");

        var isValid = _service.VerifyAndUpgradeIfNeeded(khadem, "Incorrect-Password");

        Assert.False(isValid);
    }

    [Fact]
    public void VerifyAndUpgradeIfNeeded_UpgradesMatchingLegacyPlainTextPassword()
    {
        var khadem = new Khadem { PasswordHash = "legacy-password" };

        var isValid = _service.VerifyAndUpgradeIfNeeded(khadem, "legacy-password");

        Assert.True(isValid);
        Assert.NotEqual("legacy-password", khadem.PasswordHash);
        Assert.True(_service.VerifyAndUpgradeIfNeeded(khadem, "legacy-password"));
    }

    [Fact]
    public void VerifyAndUpgradeIfNeeded_DoesNotUpgradeLegacyPasswordWhenItDoesNotMatch()
    {
        var khadem = new Khadem { PasswordHash = "legacy-password" };

        var isValid = _service.VerifyAndUpgradeIfNeeded(khadem, "wrong-password");

        Assert.False(isValid);
        Assert.Equal("legacy-password", khadem.PasswordHash);
    }
}
