using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthApiController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var token = await _authService.LoginAsync(model);
        if (token == null)
        {
            return Unauthorized(new ResponseModel { Message = "Invalid username or password" });
        }
        return Ok(new ResponseModel { Token = token });
    }

    [HttpPost("forgotpassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        var result = await _authService.ForgotPasswordAsync(model);
        if (!result)
        {
            return BadRequest(new ResponseModel { Message = "Username and Email do not match" });
        }
        return Ok(new ResponseModel { Message = "Password has been sent to your email" });
    }

    
    
    public class ResponseModel
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }

}