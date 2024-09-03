using TokenManager.Web.Api.Models.Authorities;

namespace TokenManager.Web.Api.Services.EOS
{
    public class EOSConfig
    {
        public string HttpEndpoint { get; set; }
        public string ChainId { get; set; }
        public BaseAuthority TokenIssuer { get; set; }
        public BaseAuthority AccountIssuer { get; set; }
    }
}