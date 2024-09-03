namespace TokenManager.Web.Api.Models.Authorities
{
    public class AccountAuthority : BaseAuthority
    {
        public override string Name { get; set; }
        public override string PrivateKey { get; set; }
        public override string PublicKey { get; set; }
    }
}