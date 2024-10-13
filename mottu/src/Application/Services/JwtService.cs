using CrossCutting.Common.Dtos.Response;
using CrossCutting.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;
    private readonly DateTime CreationDate = DateTime.UtcNow;

    public JwtService(
        JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;

        _key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
    }

    public JwtResponse CreateToken(
        Domain.Entities.User user)
    {
        var identity = GetClaims(user);

        var expirationDate = GetDateToExpires();

        var handler = new JwtSecurityTokenHandler();

        var securityToken = GetSecurityToken(identity, expirationDate, handler);

        var token = handler.WriteToken(securityToken);
        var refreshToken = Guid.NewGuid().ToString().Replace("-", string.Empty);

        var result = new JwtResponse(
            CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
            token,
            refreshToken);

        return result;
    }

    private static ClaimsIdentity GetClaims(
        Domain.Entities.User user)
    {
        var claimsIdentity = new ClaimsIdentity(
            new[] {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Name),
                    new Claim(ClaimTypes.Role, user.UserRole.Description),
                    new Claim("UserId", user.Id.ToString()),
            });

        if (user.Driver != null)
            claimsIdentity.AddClaim(new Claim("DriverId", user.Driver.Id.ToString()));

        return claimsIdentity;
    }

    private DateTime GetDateToExpires()
    {
        return CreationDate.AddDays(_jwtSettings.TotalHoursExpiresToken);
    }

    private SecurityToken GetSecurityToken(
        ClaimsIdentity identity,
        DateTime expirationDate,
        JwtSecurityTokenHandler handler)
    {
        return handler.CreateToken(
            new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identity,
                NotBefore = CreationDate,
                Expires = expirationDate
            });
    }
}