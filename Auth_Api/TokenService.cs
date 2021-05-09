using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Api
{
    public class TokenService
    {
        public string Generate(bool flag,string role)
        {
            if (flag)
            {
                var key = Encoding.ASCII.GetBytes("this is a secret key I am using for authentication for my capstone project api");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier,role),
                        new Claim(ClaimTypes.Role, "Admin"),
                    }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Expires = DateTime.Now.AddHours(2)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var createdtoken = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(createdtoken);
            }
            else
            {
                return null;
            }
        }
    }
}
