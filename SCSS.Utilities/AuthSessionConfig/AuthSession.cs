using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.AuthSessionConfig
{
    public class AuthSession : IAuthSession
    {
        public UserInfoSession UserSession => AuthSessionGlobalVariable.UserSession;
    }
}
