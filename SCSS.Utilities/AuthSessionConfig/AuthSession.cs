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
