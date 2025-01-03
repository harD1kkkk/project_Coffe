<<<<<<< HEAD
﻿using Microsoft.Extensions.Options;
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

        public AuthenticationService(IOptions<Jwt> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
=======
﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelRealization
{
    public class AuthenticationService
    {
        private readonly Jwt _jwtSettings;

        public AuthenticationService(Jwt jwtSettings)
        {
            _jwtSettings = jwtSettings;
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public string GenerateToken(int userId, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
