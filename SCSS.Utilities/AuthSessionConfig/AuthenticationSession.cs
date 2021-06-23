using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.AuthSessionConfig
{
    public class AuthSessionGlobalVariable
    {
        public static UserInfoSession UserSession { get; set;}
    }

    public class UserInfoSession
    {
        public Guid Id { get; set; }

        public string ClientId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool Gender { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Role { get; set; }
    }
}
