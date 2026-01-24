using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication.Common;

public record AuthenticationResult(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Token)
{
    public AuthenticationResult(User user, string token)
        : this(user.Id, user.FirstName, user.LastName, user.Email, token)
    {
    }
}