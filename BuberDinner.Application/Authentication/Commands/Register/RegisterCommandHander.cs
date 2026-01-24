using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using ErrorOr;
using MediatR;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Application.Authentication.Common;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHander : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHander(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        //Check if user already exists
        if (_userRepository.GetUserByEmail(request.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }
        //Create user (generate unique ID)
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };
        _userRepository.AddUser(user);

        //Create JWT token

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}