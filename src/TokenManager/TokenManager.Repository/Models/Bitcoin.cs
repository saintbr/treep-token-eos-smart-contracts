using System;
using System.Collections.Generic;

namespace TokenManager.Repository.Models
{
    public partial class Bitcoin
    {
        public Bitcoin()
        {
        }

        public long BitcoinId { get; set; }
        public string Address { get; set; }
        public string Callback { get; set; }
        public int Index { get; set; }
        public int Gap { get; set; }
        public decimal TotalReceived { get; set; }
        public decimal? Cotacao { get; set; }
        public string Json { get; set; }
        public long? ContaId { get; set; }
        public DateTime DataGeracao { get; set; }
        public bool Processado { get; set; }
        public DateTime DataVinculoConta { get; set; }

        public virtual Conta Conta { get; set; }
    }
}
