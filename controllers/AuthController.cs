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

        var user = new User
        {
            Username = dto.Username,
            Password = dto.Password
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(new { message = "Register sukses" });
    }


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new { message = "Validasi gagal", errors });
        }

        var user = _context.Users.FirstOrDefault(u =>
            u.Username == dto.Username && u.Password == dto.Password);

        if (user == null)
        {
            return Unauthorized(new { message = "Username atau password salah" });
        }

        var token = _jwt.GenerateToken(user.Username);

        return Ok(new { token });
    }

}
