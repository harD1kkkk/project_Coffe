using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_Coffe.Models.ModelRealization
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Jwt _jwtSettings;
        private readonly ILogger<OrderService> _logger;

        public AuthenticationService(IOptions<Jwt> jwtSettings, ILogger<OrderService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;

        }

        public string GenerateToken(int userId, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
                {
                    throw new ArgumentNullException(nameof(_jwtSettings.SecretKey), "JWT Secret Key is missing.");
                }
                if (string.IsNullOrEmpty(_jwtSettings.Issuer))
                {
                    throw new ArgumentNullException(nameof(_jwtSettings.Issuer), "JWT Issuer is missing.");
                }
                if (string.IsNullOrEmpty(_jwtSettings.Audience))
                {
                    throw new ArgumentNullException(nameof(_jwtSettings.Audience), "JWT Audience is missing.");
                }

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation($"Token generated for user {userId} with role {role}: {tokenString}");
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating token: {ex.Message}");
                throw new Exception("An error occurred while generating the token.", ex);
            }
        }
    }
}
