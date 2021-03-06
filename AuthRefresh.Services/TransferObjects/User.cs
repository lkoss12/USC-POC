using System.Collections.Generic;
using AuthRefresh.Services.Interfaces;

namespace AuthRefresh.Services.TransferObjects
{
    public class User : IUser
    {
        public string UscId {get; set;}
        public IEnumerable<string> Claims {get; set;}
    }
}