using System;

namespace SCSS.Utilities.AuthSessionConfig
{
    public class UserInfoSession
    {
        public Guid Id { get; set; }

        public string ClientId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int? Gender { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int Role { get; set; }

        public string DeviceId { get; set; }
    }
}
