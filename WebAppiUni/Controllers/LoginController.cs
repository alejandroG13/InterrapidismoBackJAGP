using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppiUni.Data;
using WebAppiUni.DTOs.Login;
using WebAppiUni.Models;

namespace WebAppiUni.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UniversidadContext _context;
        private readonly IPasswordHasher<Estudiante> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthController(
            UniversidadContext context,
            IPasswordHasher<Estudiante> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.Email == dto.Email);

            if (estudiante == null)
                return Unauthorized("Credenciales inválidas");

            var result = _passwordHasher.VerifyHashedPassword(
                estudiante,
                estudiante.Password,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Credenciales inválidas");

            var token = GenerateJwt(estudiante);

            return Ok(new
            {
                token
            });
        }

        // Metodo para generar el JWT
        private string GenerateJwt(Estudiante estudiante)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, estudiante.IdEstudiante.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, estudiante.Email),
            new Claim("nombre", estudiante.Nombres)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpiresInMinutes"]!)
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}