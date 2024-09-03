using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenManager.Repository.Data;
using TokenManager.Web.Api.Handlers.VOs;
using TokenManager.Web.Api.Models.Tokens;
using TokenManager.Web.Api.Services.Stellar;

namespace TokenManager.Web.Api.Handlers
{
    public class GenbitComprasTreepTokenHandler : QueuedHandler
    {
        private readonly StellarBlockchainService _stellarBlockchainService;

        public GenbitComprasTreepTokenHandler(ILogger<QueuedHandler> logger, StellarBlockchainService stellarBlockchainService) : base(logger)
        {
            _stellarBlockchainService = stellarBlockchainService;
        }

        public override async Task DoWork()
        {
            var creditos = new List<PendingTransactionsVO>();
            using (var ctx = new GenbitDB_Context())
            {
                var query = "SELECT TOP 25 * FROM GenbitComprasTreepToken ORDER BY Valor ASC";
                using (var command = ctx.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandTimeout = 60000;
                    ctx.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                creditos.Add(new PendingTransactionsVO
                                {
                                    ContaId = reader.GetInt64(0),
                                    LancamentoId = reader.GetInt64(1),
                                    TipoTransacao = reader.GetString(2),
                                    Cotacao = reader.GetDecimal(3),
                                    Valor = reader.GetDecimal(4),
                                    Unstaked = reader.GetDecimal(5),
                                    MoedaId = reader.GetInt64(6),
                                    Simbolo = (TokenSymbol)Enum.Parse(typeof(TokenSymbol), reader.GetString(7)),
                                    Descricao = reader.GetString(8),
                                    UrlOrigem = reader.GetString(9),
                                    UrlDestino = reader.GetString(10),
                                    Extra = reader.GetString(11)
                                });
                            }
                        }
                    }
                }
            }

            foreach (var credito in creditos)
            {
                var account = await _stellarBlockchainService.GetAccount(credito.UrlDestino);
                if (!account.Balances.ToList().Any(b => b.AssetCode == "TREEP"))
                {
                    await _stellarBlockchainService.TrustToken(credito.UrlDestino, credito.Extra);
                }

                if (!account.Signers.ToList().Any(s => s.Key == "GDTG7XG7OCVBMDYK3ZXW7HSB2LHTS4MZF4AD4DDHVML2NCPPVYWBNDR6"))
                {
                    await _stellarBlockchainService.SignAccount(credito.UrlDestino, credito.Extra);
                }

                var amount = Convert.ToInt64(Math.Round(credito.Valor, 7).ToString().Replace(",", ""));
                var result = await _stellarBlockchainService.Payment(credito.UrlOrigem, credito.UrlDestino, amount, credito.Descricao);
                switch (result.Item1)
                {
                    case true:
                        {
                            var MoedaId = new SqlParameter("@MoedaId", credito.MoedaId);
                            var ContaId = new SqlParameter("@ContaId", credito.ContaId);
                            var Simbolo = new SqlParameter("@Simbolo", credito.Simbolo);
                            var LancamentoId = new SqlParameter("@LancamentoId", credito.LancamentoId);
                            var Tx = new SqlParameter("@Tx", result.Item2);
                            var Valor = new SqlParameter("@Valor", credito.Valor);
                            using var ctx = new GenbitDB_Context();
                            ctx.Database.ExecuteSqlRaw("INSERT INTO TransacaoTokenStellar ([Hash], DataCadastro, MoedaId, ContaId, Simbolo, LancamentoId, Tx, Valor, Taxa, Confirmado) VALUES(NEWID(), GETDATE(), @MoedaId, @ContaId, @Simbolo, @LancamentoId, @Tx, @Valor, 0, 0); ", MoedaId, ContaId, Simbolo, LancamentoId, Tx, Valor);
                        }
                        break;

                    case false:
                        await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(result, Formatting.Indented));
                        break;
                }
            }

            await Task.CompletedTask;
        }
    }
}