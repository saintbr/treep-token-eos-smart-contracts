using System;
using System.Collections.Generic;

namespace TokenManager.Repository.Models
{
    public partial class Carteira
    {
        public Carteira()
        {
        }

        public long CarteiraId { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public long MoedaId { get; set; }
        public int StatusCarteira { get; set; }
        public DateTime DataCadastro { get; set; }
        public long ContaId { get; set; }
        public Guid Hash { get; set; }
        public DateTime? DataConfirmacao { get; set; }
        public int TipoCarteira { get; set; }

        public virtual Conta Conta { get; set; }
        public virtual Moeda Moeda { get; set; }
    }
}
