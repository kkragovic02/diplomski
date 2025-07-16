using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Zora.Core.Config;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.AuthService;
using Zora.Core.Models;

namespace Zora.Core.Features.Auth;

internal class AuthService(
    ZoraDbContext dbContext,
    ILogger<AuthService> logger,
    PasswordHasher<UserModel> passwordHasher,
    IOptions<JwtSettings> jwtOptions
) : IAuthService
{
    private readonly JwtSettings _jwt = jwtOptions.Value;

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var userModel = await dbContext.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email,
            cancellationToken
        );
        if (userModel == null)
        {
            logger.LogWarning("Login attempt for non-existing user {Email}", request.Email);
            return null;
        }

        var result = passwordHasher.VerifyHashedPassword(
            userModel,
            userModel.PasswordHash!,
            request.Password
        );
        if (result == PasswordVerificationResult.Failed)
        {
            logger.LogWarning("Wrong pasword for user {Email}", request.Email);
            return null;
        }

        var token = GenerateJwtToken(userModel, request.RememberMe);

        return new LoginResponse(token, userModel.MapToUser());
    }

    public async Task<User> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        var userModel = new UserModel
        {
            Name = request.Name,
            Email = request.Email,
            Role = Role.Member,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        userModel.PasswordHash = passwordHasher.HashPassword(userModel, request.Password);

        dbContext.Users.Add(userModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return userModel.MapToUser();
    }

    private string GenerateJwtToken(UserModel user, bool rememberMe)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var expires = DateTime.UtcNow.AddDays(rememberMe ? _jwt.ExpiresInDays : 1);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
