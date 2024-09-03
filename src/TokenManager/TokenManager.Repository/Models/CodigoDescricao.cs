using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokenManager.Repository.Models
{
    [NotMapped]
    public partial class CodigoDescricao
    {
        public long Codigo { get; set; }
        public string Descricao { get; set; }
    }
}
