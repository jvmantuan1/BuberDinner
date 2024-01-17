using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using DomainErrors = BuberDinner.Domain.Common.Errors.Errors;

namespace BuberDinner.Api.Controllers
{
    [Route("auth")]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest req)
        {
            var registerResult = _authService.Register(
                req.FirstName,
                req.LastName,
                req.Email,
                req.Password);

            return registerResult.Match(authResult => 
                Ok(MapAuthResult(authResult)),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest req)
        {
            var authResult = _authService.Login(req.Email, req.Password);

            if(authResult.IsError && authResult.FirstError == DomainErrors.Authentication.InvalidCredentials)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description);
            }
            return authResult.Match(authResult => 
                Ok(MapAuthResult(authResult)),
                errors => Problem(errors));
        }

        private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(authResult.User.Id,
                                authResult.User.FirstName,
                                authResult.User.LastName,
                                authResult.User.Email,
                                authResult.Token);
        }
    }
}