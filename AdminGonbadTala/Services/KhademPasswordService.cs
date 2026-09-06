using System.Security.Cryptography;
using System.Text;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminGonbadTala.Services;

public sealed class KhademPasswordService
{
    private readonly IPasswordHasher<Khadem> _passwordHasher;

    public KhademPasswordService(IPasswordHasher<Khadem> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public void SetPassword(Khadem khadem, string password)
    {
        khadem.PasswordHash = _passwordHasher.HashPassword(khadem, password);
    }

    public bool VerifyAndUpgradeIfNeeded(Khadem khadem, string password)
    {
        var verification = VerifyHashedPassword(khadem, password);

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            SetPassword(khadem, password);
            return true;
        }

        if (verification != PasswordVerificationResult.Failed)
        {
            return true;
        }

        if (!IsLegacyPasswordMatch(khadem.PasswordHash, password))
        {
            return false;
        }

        SetPassword(khadem, password);
        return true;
    }

    private PasswordVerificationResult VerifyHashedPassword(Khadem khadem, string password)
    {
        try
        {
            return _passwordHasher.VerifyHashedPassword(khadem, khadem.PasswordHash, password);
        }
        catch (FormatException)
        {
            // Values stored before password hashing are not valid Base64 payloads.
            return PasswordVerificationResult.Failed;
        }
    }

    private static bool IsLegacyPasswordMatch(string storedValue, string suppliedPassword)
    {
        if (string.IsNullOrEmpty(storedValue))
        {
            return false;
        }

        var storedBytes = Encoding.UTF8.GetBytes(storedValue);
        var suppliedBytes = Encoding.UTF8.GetBytes(suppliedPassword);
        return storedBytes.Length == suppliedBytes.Length &&
               CryptographicOperations.FixedTimeEquals(storedBytes, suppliedBytes);
    }
}
