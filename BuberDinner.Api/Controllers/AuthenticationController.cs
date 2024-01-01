using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest req)
        {
            var authResult = _authService.Register(req.FirstName, req.LastName, req.Email, req.Password);

            var response = new AuthenticationResponse(authResult.Id, authResult.FirstName, authResult.LastName,
                authResult.Email, authResult.Token);
            
            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest req)
        {
            var authResult = _authService.Login(req.Email, req.Password);
            
            var response = new AuthenticationResponse(authResult.Id, authResult.FirstName, authResult.LastName,
                authResult.Email, authResult.Token);
            
            return Ok(response);
        }
    }
}