using TokenManager.Web.Api.Models.Tokens;

namespace TokenManager.Web.Api.Handlers.VOs
{
    public class PendingTransactionsVO
    {
        public long ContaId { get; set; }
        public long LancamentoId { get; set; }
        public string TipoTransacao { get; set; }
        public decimal Cotacao { get; set; }
        public decimal Valor { get; set; }
        public decimal Unstaked { get; set; }
        public long MoedaId { get; set; }
        public TokenSymbol Simbolo { get; set; }
        public string Descricao { get; set; }
        public string UrlOrigem { get; set; }
        public string UrlDestino { get; set; }
        public string Extra { get; set; }
    }
}