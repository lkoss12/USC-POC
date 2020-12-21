namespace AuthRefresh.Data.Contexts.AuthRefresh.Models
{
    public class RoleClaim
    {
        public int RoleClaimId {get; set;}
        public int RoleId {get; set;}
        public int ClaimId {get; set;}

        public Role Role {get; set;}
        public Claim Claim {get; set;}
    }
}