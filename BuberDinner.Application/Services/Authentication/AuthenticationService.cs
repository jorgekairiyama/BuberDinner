using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        //Check if user already exists
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new DuplicateEmailException();
        }
        //Create user (generate unique ID)
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
        _userRepository.AddUser(user);

        //Create JWT token

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            firstName,
            lastName);

        return new AuthenticationResult(
            user.Id,
            firstName,
            lastName,
            email,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        //Check if user exists
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("User with given email does not exist");
        }
        //Validate password
        if (user.Password != password)
        {
            throw new Exception("Invalid password");
        }

        //Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.FirstName,
            user.LastName);

        return new AuthenticationResult(
            user.Id,
            user.FirstName,
            user.LastName,
            email,
            token);
    }
}