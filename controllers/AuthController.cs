using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApi.models;
using ProductApi.dtos;
using ProductApi.data;
using ProductApi.services;
using System.Linq;
using Microsoft.AspNetCore.Identity;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwt;

    public AuthController(AppDbContext context, JwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new { message = "Validasi gagal", errors });
        }

        try
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
            {
                return BadRequest(new { message = "Username sudah digunakan" });
            }

            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                Username = dto.Username
            };

            
            user.Password = passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Register sukses" });
        }
        catch (Exception ex)
        {
            var inner = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

            return StatusCode(500, new
            {
                status = "fail",
                message = "Terjadi kesalahan di server",
                error = inner
            });
        }

    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == dto.Username);
        if (user == null)
        {
            return BadRequest(new { message = "Username tidak ditemukan" });
        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return BadRequest(new { message = "Username atau password salah" });
        }

        // Jika password cocok, proses login dan buat token
        var token = _jwt.GenerateToken(user.Username, user.Id); 
        return Ok(new { message = "Login sukses", token });
    }


}
