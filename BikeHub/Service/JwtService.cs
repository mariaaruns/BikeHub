using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace BikeHub.Service
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GenerateJwt(UserDetails user)
        {

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)];


            var key = _configuration.GetValue<string>("jwt:SecureKey");
            var keyByteArray = Encoding.UTF8.GetBytes(key);
            var securitykey = new SymmetricSecurityKey(keyByteArray);
            var signingCreds = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("jwt:Issuer"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signingCreds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return jwt;
        }
    }
}
