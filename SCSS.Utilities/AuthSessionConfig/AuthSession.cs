using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.AuthSessionConfig
{
    public class AuthSession : IAuthSession
    {
        public UserInfoSession UserSession { get; private set; }

        public void SetUserInfoSession(UserInfoSession user)
        {
            UserSession = user;
        }
    }
}
