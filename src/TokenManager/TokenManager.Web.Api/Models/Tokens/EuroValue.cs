using TokenManager.Web.Api.Models.Authorities;

namespace TokenManager.Web.Api.Models.Tokens
{
    public class EuroValue : TokenDefinition
    {
        public override string Name { get; set; } = "Euro";
        public override TokenSymbol Symbol { get; set; } = TokenSymbol.EurValue;
        public override BaseAuthority Issuer { get => base.Issuer; set => base.Issuer = value; }
        public override decimal Supply { get; set; } = 100000000m;
    }
}