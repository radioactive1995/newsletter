using Ardalis.GuardClauses;
using Domain.Common;

namespace Domain.Subscribers;
public class Subscriber : Entity<int>
{
    public required string Email { get; set; }

    public static Subscriber CreateEntity(string email)
    {
        Guard.Against.NullOrWhiteSpace(email, parameterName: nameof(Email), message: "Subscriber.Email cannot be null or empty");

        return new Subscriber()
        {
            Email = email 
        };
    }
}
