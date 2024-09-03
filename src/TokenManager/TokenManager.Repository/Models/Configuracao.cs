using System;
using System.Collections.Generic;

namespace TokenManager.Repository.Models
{
    public partial class Configuracao
    {
        public Configuracao()
        {
        }

        public long ConfiguracaoId { get; set; }
        public decimal TaxaDepositoBrl { get; set; }
        public decimal TaxaDepositoBrlnaoConveniado { get; set; }
        public decimal TaxaDepositoBtc { get; set; }
        public decimal TarifaSaqueBrl { get; set; }
        public decimal TaxaSaqueBrl { get; set; }
        public decimal TarifaSaqueBrlnaoConveniado { get; set; }
        public decimal TaxaSaqueBrlnaoConveniado { get; set; }
        public decimal TaxaSaqueBtc { get; set; }
        public decimal TaxaOp { get; set; }
        public decimal TaxaOa { get; set; }
        public decimal IndiceCotacao { get; set; }
        public bool? Ativo { get; set; }
        public decimal TaxaTransferenciaExterna { get; set; }
        public decimal TaxaTransferenciaInterna { get; set; }
        public decimal LimiteCompraAutomaticaBtc { get; set; }
        public bool? DocumentoGenbitZero10 { get; set; }
        public decimal ValorTransferenciaAutomaticaBtc { get; set; }
        public bool? ValidacaoSms { get; set; }
        public DateTime DataCadastro { get; set; }
        public decimal TaxaPagamento { get; set; }
        public decimal TaxaDepositoBtcAtm { get; set; }
        public decimal TaxaSaqueMoedaFisicaAtm { get; set; }
        public decimal TarifaSaqueMoedaFisicaAtm { get; set; }
        public decimal TaxaSaqueBtcAtm { get; set; }
        public decimal TaxaTransferenciaExternaAtm { get; set; }
        public decimal IndiceCotacaoAtm { get; set; }
        public bool? ValidacaoSmsAtm { get; set; }
        public decimal ComissaoCompraBtcAtm { get; set; }
        public decimal ComissaoVendaBtcAtm { get; set; }
        public decimal TigerMoney { get; set; }
        public decimal TaxaCotacaoCripto { get; set; }

    }
}
