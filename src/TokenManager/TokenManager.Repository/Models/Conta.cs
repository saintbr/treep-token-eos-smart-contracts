using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TokenManager.Repository.Models
{
    public partial class Conta
    {
        public Conta()
        {
            Bitcoins = new HashSet<Bitcoin>();
            Carteiras = new HashSet<Carteira>();
        }

        public long ContaId { get; set; }
        public bool EmailConfirmado { get; set; }
        public bool ContaValidada { get; set; }
        public long? MoedaId { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Bitcoin> Bitcoins { get; set; }
        public virtual ICollection<Carteira> Carteiras { get; set; }
    }
}
