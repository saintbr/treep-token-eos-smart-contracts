using TokenManager.Web.Api.Models.Authorities;

namespace TokenManager.Web.Api.Models.Tokens
{
    public class TreepToken : TokenDefinition
    {
        public override string Name { get; set; } = "TreepToken";
        public override TokenSymbol Symbol { get; set; } = TokenSymbol.TPK;
        public override int Decimals { get; set; } = 4;
        public override BaseAuthority Issuer { get => base.Issuer; set => base.Issuer = value; }
        public override decimal Supply { get; set; } = 15000000000m;
    }
}