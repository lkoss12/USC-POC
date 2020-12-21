using AuthRefreshApi.Interfaces;

namespace AuthRefreshApi.TransferObjects
{
    public class User : IUser
    {
        public string UscId {get; set;}
    }
}