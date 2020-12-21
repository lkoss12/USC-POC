using AuthRefresh.Services.Interfaces;


namespace AuthRefresh.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUser _user;
        private readonly IImpersonatedUser _impersonatedUser;
        public UserService(IUser user,
                           IImpersonatedUser impersonatedUser)
        {
            _user = user;
            _impersonatedUser = impersonatedUser;
        }
    }
}