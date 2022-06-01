using CommonLayer;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        public void AddUser(UserPostModel user);

        public string LoginUser(string email, string password);
        public bool ForgotPassword(string email);
        public bool ChangePassword(ChangePasswordModel changepassword, string email);
    }
}
