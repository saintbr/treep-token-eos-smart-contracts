namespace TokenManager.Web.Api.Models.Request.API
{
    public class TransferRequest
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public decimal Quantity { get; set; }
        public decimal Unstaked { get; set; }
        public string Memo { get; set; }
    }
}