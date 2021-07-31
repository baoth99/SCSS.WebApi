using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.AuthSessionConfig
{
    public interface IAuthSession
    {
        public UserInfoSession UserSession { get; }

        void SetUserInfoSession(UserInfoSession user);
    }
}
