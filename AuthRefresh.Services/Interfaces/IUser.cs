using System.Collections.Generic;

namespace AuthRefresh.Services.Interfaces
{
    public interface IUser
    {
        string UscId {get; set;}
        IEnumerable<string> Claims {get; set;}
    }
}