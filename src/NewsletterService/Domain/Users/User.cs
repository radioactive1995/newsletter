using Ardalis.GuardClauses;
using Domain.Common;

namespace Domain.Users;

public class User : Entity<int>
{
    public required List<string> Emails { get; set; }

    public string? LocalId { get; set; }
    public string? MicrosoftId { get; set; }
    public string? GoogleId { get; set; }


    public static User CreateEntity(AccountType accountType, string oidClaim, string[] emails)
    {
        Guard.Against.NullOrEmpty(emails, nameof(Emails), message: "User.CreateEntity.Emails cannot be null or empty");
        Guard.Against.NullOrWhiteSpace(oidClaim, nameof(oidClaim), message: "User.CreateEntity.oidClaim cannot be null or empty");

        return new User()
        {
            Emails = emails.ToList(),
            LocalId = accountType == AccountType.Local ? oidClaim : null,
            MicrosoftId = accountType == AccountType.Microsoft ? oidClaim : null,
            GoogleId = accountType == AccountType.Google ? oidClaim : null
        };
    }

    public void UpdateUserSettings(AccountType accountType, string oidClaim, string[] emails)
    {
        Guard.Against.NullOrWhiteSpace(oidClaim, nameof(oidClaim), message: "User.UpdateUserSettings.oidClaim cannot be null or empty");

        Emails = Emails.Union(emails.ToList()).ToList();

        LocalId = accountType == AccountType.Local ? oidClaim : LocalId;
        MicrosoftId = accountType == AccountType.Microsoft ? oidClaim : MicrosoftId;
        GoogleId = accountType == AccountType.Google ? oidClaim : GoogleId;
    }

    public enum AccountType
    {
        Local,
        Microsoft,
        Google,
    };
}
