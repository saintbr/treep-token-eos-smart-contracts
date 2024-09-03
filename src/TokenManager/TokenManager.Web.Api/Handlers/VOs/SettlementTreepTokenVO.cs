namespace TokenManager.Web.Api.Handlers.VOs
{
    public class SettlementTreepTokenVO
    {
        public long ContaId { get; set; }
        public string Simbolo { get; set; }
        public string Carteira { get; set; }
        public decimal Saldo { get; set; }
        public int Transacoes { get; set; }
    }
}