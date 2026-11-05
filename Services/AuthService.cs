using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CampusPlacementManagement.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            // 1. Find user by email
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return null;

            // 2. Verify password
            var isPasswordValid = await _userManager.CheckPasswordAsync(
                user, request.Password);

            if (!isPasswordValid)
                return null;

            // 3. Get user's roles
            var roles = await _userManager.GetRolesAsync(user);

            // 4. Create JWT claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 5. Create signing key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            // 6. Set token expiry
            var expiresAt = DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpiryInMinutes);

            // 7. Generate JWT
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            // 8. Return authentication response
            return new AuthResponse
            {
                Token = tokenString,
                ExpiresAt = expiresAt,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Roles = roles
            };
        }
    }
}