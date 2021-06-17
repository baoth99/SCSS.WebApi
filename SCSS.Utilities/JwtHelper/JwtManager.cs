using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.JwtHelper
{
    public class JwtManager
    {
        public static void ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSercurity = tokenHandler.ReadJwtToken(token);

            var identityClaim = jwtSercurity.Claims;


        }
    }
}
