using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TwitterBackend
{
    public class JwtCreator
    {
        private readonly AuthOptions _authOptions;
        public JwtCreator(AuthOptions authOptions)
        {
            _authOptions = authOptions;
        }

        public string Create(string nickName, string role)
        {
            var claims = new List<Claim> { new(ClaimTypes.Name, nickName), new(ClaimTypes.Role, role) };

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}