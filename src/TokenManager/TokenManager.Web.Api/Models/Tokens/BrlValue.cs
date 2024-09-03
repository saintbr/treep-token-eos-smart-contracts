using TokenManager.Web.Api.Models.Authorities;

namespace TokenManager.Web.Api.Models.Tokens
{
    public class BrlValue : TokenDefinition
    {
        public override string Name { get; set; } = "Brazilian Real";
        public override TokenSymbol Symbol { get; set; } = TokenSymbol.BrlValue;
        public override BaseAuthority Issuer { get => base.Issuer; set => base.Issuer = value; }
        public override decimal Supply { get; set; } = 100000000m;
    }
}