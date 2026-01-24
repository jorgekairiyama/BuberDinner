using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string firstName, string lastName);
    string GenerateToken(User user);
}
