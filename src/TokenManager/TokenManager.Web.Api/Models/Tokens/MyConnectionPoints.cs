using TokenManager.Web.Api.Models.Authorities;

namespace TokenManager.Web.Api.Models.Tokens
{
    public class MyConnectionPoints : TokenDefinition
    {
        public override string Name { get; set; } = "My Connection Points";
        public override TokenSymbol Symbol { get; set; } = TokenSymbol.CNPOINT;
        public override BaseAuthority Issuer { get => base.Issuer; set => base.Issuer = value; }
        public override decimal Supply { get; set; } = 100000000m;
    }
}