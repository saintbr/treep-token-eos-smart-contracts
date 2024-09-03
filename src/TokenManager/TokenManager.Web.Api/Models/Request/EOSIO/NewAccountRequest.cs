namespace TokenManager.Web.Api.Models.Request.EOSIO
{
    public class NewAccountRequest
    {
        public NewAccountAutority active { get; set; }

        public string creator { get; set; }

        public string name { get; set; }

        public NewAccountAutority owner { get; set; }
    }
}