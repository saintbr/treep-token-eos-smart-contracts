using System;
using System.Collections.Generic;

namespace TokenManager.Repository.Models
{
    public partial class Moeda
    {
        public Moeda()
        {
            Carteira = new HashSet<Carteira>();
        }

        public long MoedaId { get; set; }
        public string Nome { get; set; }
        public string CodigoAlfabetico { get; set; }
        public string Simbolo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public int TipoMoeda { get; set; }

        public virtual ICollection<Carteira> Carteira { get; set; }
    }
}
