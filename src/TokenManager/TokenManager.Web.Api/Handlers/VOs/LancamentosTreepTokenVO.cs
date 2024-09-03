using TokenManager.Web.Api.Models.Tokens;

namespace TokenManager.Web.Api.Handlers.VOs
{
    public class LancamentosTreepTokenVO
    {
        public long ContaId { get; set; }
        public long LancamentoId { get; set; }
        public int StatusLancamento { get; set; }
        public decimal Valor { get; set; }
        public decimal Unstaked { get; set; }
        public string Url { get; set; }
        public long MoedaId { get; set; }
        public TokenSymbol Simbolo { get; set; }
        public string Descricao { get; set; }
    }
}