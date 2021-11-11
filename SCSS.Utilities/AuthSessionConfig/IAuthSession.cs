
namespace SCSS.Utilities.AuthSessionConfig
{
    public interface IAuthSession
    {
        public UserInfoSession UserSession { get; }

        void SetUserInfoSession(UserInfoSession user);
    }
}
