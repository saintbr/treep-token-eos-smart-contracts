namespace TokenManager.Web.Api.Models.Request.EOSIO
{
    public partial class NewAccountAutorityKey
    {
        public string key { get; set; }

        public long weight { get; set; }
    }
}