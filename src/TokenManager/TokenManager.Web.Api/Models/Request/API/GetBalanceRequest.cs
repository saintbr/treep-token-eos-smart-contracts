namespace TokenManager.Web.Api.Models.Request.API
{
    public class GetBalanceRequest
    {
        public string Address { get; set; }

        public string Token { get; set; }
    }
}