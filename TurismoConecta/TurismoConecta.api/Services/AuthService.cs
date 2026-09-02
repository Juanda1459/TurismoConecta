using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.DTOs.Auth;
using TurismoConecta.api.Models;
using TurismoConecta.api.Services.Interfaces;
using System.Threading.Tasks;
using System;

namespace TurismoConecta.api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    private readonly PasswordHasher<Usuario> _hasher = new();

    public AuthService(AppDbContext db, JwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    } 

    public async Task<AuthResponseDto> RegistrarAsync(RegisterRequestDto dto)
    {
        bool emailExiste = await _db.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (emailExiste) throw new InvalidOperationException("El email ya está registrado.");

        var rolUsuario = await _db.Rols.FirstOrDefaultAsync(r => r.Nombre == "Usuario")
            ?? throw new InvalidOperationException("Rol 'Usuario' no encontrado.");

        var nuevoUsuario = new Usuario
        {
            Nombre           = dto.Nombre,
            Apellido         = dto.Apellido,
            Email            = dto.Email,
            Telefono         = dto.Telefono,
            IdRol            = rolUsuario.IdRol,
            FechaRegistro    = DateTime.UtcNow,
            EmailConfirmado  = false,
            Activo           = true,
            PasswordHash     = string.Empty
        };

        nuevoUsuario.PasswordHash = _hasher.HashPassword(nuevoUsuario, dto.Password);

        _db.Usuarios.Add(nuevoUsuario);
        await _db.SaveChangesAsync();

        nuevoUsuario.IdRolNavigation = rolUsuario;

        var (token, expira) = _jwt.GenerarToken(nuevoUsuario);
        return new AuthResponseDto
        {
            Token         = token,
            Expira        = expira,
            IdUsuario     = nuevoUsuario.IdUsuario,
            NombreCompleto = $"_{nuevoUsuario.Nombre} {nuevoUsuario.Apellido}",
            Email         = nuevoUsuario.Email,
            Rol           = rolUsuario.Nombre
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await _db.Usuarios.Include(u => u.IdRolNavigation).FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuario == null || !usuario.Activo) throw new UnauthorizedAccessException("Credenciales inválidas.");

        var resultado = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password);
        if (resultado == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Credenciales inválidas.");

        var (token, expira) = _jwt.GenerarToken(usuario);
        return new AuthResponseDto
        {
            Token          = token,
            Expira         = expira,
            IdUsuario      = usuario.IdUsuario,
            NombreCompleto = $"_{usuario.Nombre} {usuario.Apellido}",
            Email          = usuario.Email,
            Rol            = usuario.IdRolNavigation.Nombre
        };
    }
}
