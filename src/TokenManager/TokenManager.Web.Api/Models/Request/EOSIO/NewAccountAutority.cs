using System.Collections.Generic;

namespace TokenManager.Web.Api.Models.Request.EOSIO
{
    public partial class NewAccountAutority
    {
        public List<object> accounts { get; set; }

        public List<NewAccountAutorityKey> keys { get; set; }

        public long threshold { get; set; }

        public List<object> waits { get; set; }
    }
}