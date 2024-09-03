namespace TokenManager.Web.Api.Services.Stellar
{
    public class StellarConfig
    {
        public string HttpEndpoint { get; set; }
        public StellarConfigAuthority TokenIssuer { get; set; }
        public StellarConfigAuthority AccountIssuer { get; set; }
    }
}