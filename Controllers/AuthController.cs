using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
     public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        bool isAuthenticated = AuthenticateUser(model.Username, model.Password);

        if (isAuthenticated)
        {  
            var token = GenerateJwtToken(model.Username);
            return Ok(new { Token = token });
        }
  
        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
         bool isRegistered = RegisterUser(model.Username, model.Password);

        if (isRegistered)
        {
         var token = GenerateJwtToken(model.Username);

            return Ok(new { Token = token });
        }
   
        return Conflict(); 
    }

    private bool AuthenticateUser(string username, string password)
    {  
        return (username == "exampleUser" && password == "examplePassword");
    }

    private bool RegisterUser(string username, string password)
    {
      
        return true; 
    }

    private string GenerateJwtToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1), 
            signingCredentials: credentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}
public class RegisterModel
{   public string Username { get; internal set; }
    public string Password { get; internal set; }
}

public class LoginModel
{   public string Username { get; internal set; }
    public string Password { get; internal set; }
}