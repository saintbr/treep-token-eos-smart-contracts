namespace TokenManager.Web.Api.Models.Response.API
{
    public class AccountBalanceInfo
    {
        public string Symbol { get; set; }
        public decimal Balance { get; set; }
        public decimal Staked { get; set; }
    }
}