using Banking_Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Banking_Api.Jwt
{
    public class JwtServices
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _JwtKey;



        public JwtServices(IConfiguration configuration) 
        {

            _configuration = configuration;

            // jwtKey for both encripting and descripting,  this will transfer the key string into bytes and SymmetricSecurityKe will create a key
            _JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        }


        public  string CreateJWT(User user)
        {

            //claims inside our token
            var userClaims = new List<Claim> 
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.GivenName, user.Firstname),
                 new Claim(ClaimTypes.Surname, user.Lastname)


            };

            //sign in creasentil which will take in the key and algorith
            var creadentials = new SigningCredentials(_JwtKey, SecurityAlgorithms.HmacSha512Signature);
            
            //the description of our takens
            var tokenDescriper = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:ExpiresInDays"])),
                SigningCredentials = creadentials,
                Issuer = _configuration["JWT:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
           
            //creating our token using token descriper
            var jwt = tokenHandler.CreateToken(tokenDescriper);


            return tokenHandler.WriteToken(jwt);
        }
    }
}
