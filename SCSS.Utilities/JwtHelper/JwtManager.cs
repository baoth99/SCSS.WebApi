using IdentityModel;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.Helper;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SCSS.Utilities.JwtHelper
{
    public class JwtManager
    {
        public static UserInfoSession ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSercurity = tokenHandler.ReadJwtToken(token);

            var identityClaim = jwtSercurity.Claims.ToList();

            identityClaim.RemoveAll(x => x.Type == "scope");

            var dictionaryClaim = identityClaim.ToDictionary(x => x.Type, x => x.Value);

            var userSession = new UserInfoSession();

            userSession.Id = Guid.Parse(dictionaryClaim[JwtClaimTypes.Subject]);
            userSession.Name = dictionaryClaim[JwtClaimTypes.Name];
            userSession.Phone = dictionaryClaim[JwtClaimTypes.PhoneNumber];
            userSession.Role = CommonUtils.GetRole(dictionaryClaim[JwtClaimTypes.Role]);
            userSession.Gender = CommonUtils.GetGender(dictionaryClaim[JwtClaimTypes.Gender]);
            userSession.ClientId = dictionaryClaim[JwtClaimTypes.ClientId];
            userSession.Email = dictionaryClaim[JwtClaimTypes.Email];
            userSession.Address = dictionaryClaim[JwtClaimTypes.Address];

            return userSession;
        }
    }
}
