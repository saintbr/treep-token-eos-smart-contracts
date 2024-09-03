using TokenManager.Web.Api.Models.Authorities;

namespace TokenManager.Web.Api.Models.Tokens
{
    public abstract class TokenDefinition
    {
        public virtual string Name { get; set; }
        public virtual TokenSymbol Symbol { get; set; }
        public virtual int Decimals { get; set; } = 0;
        public virtual BaseAuthority Issuer { get; set; } = new TokenAuthority { };
        public virtual decimal Supply { get; set; }
    }
}