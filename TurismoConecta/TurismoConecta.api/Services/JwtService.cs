using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using TurismoConecta.api.Models;

namespace TurismoConecta.api.Services;

public class JwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config) { _config = config; }

    public (string token, DateTime expira) GenerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Email,          usuario.Email),
            new(ClaimTypes.Name,           $"{usuario.Nombre} {usuario.Apellido}"),
            new(ClaimTypes.Role,           usuario.IdRolNavigation.Nombre)
        };

        var claveSecreta = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no está configurada.");
        var key  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expira = DateTime.UtcNow.AddHours(int.Parse(_config["Jwt:ExpiraHoras"] ?? "8"));

        var tokenDescriptor = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            expira,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return (tokenString, expira);
    }
}
