namespace AuthRefresh.Data.Contexts.AuthRefresh.Models
{
    public class UserProtectedData
    {
        public int UserProtectedDataId {get; set;}
        public int UserId {get; set;}
        public int ProtectedDataId {get; set;}

        public User User {get; set;}
        public ProtectedData ProtectedData {get; set;}
    }
}