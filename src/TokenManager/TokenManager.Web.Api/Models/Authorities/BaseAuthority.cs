namespace TokenManager.Web.Api.Models.Authorities
{
    public class BaseAuthority
    {
        public virtual string Name { get; set; }
        public virtual string PrivateKey { get; set; }
        public virtual string PublicKey { get; set; }
    }
}