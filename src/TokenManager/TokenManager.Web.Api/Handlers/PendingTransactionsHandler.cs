using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenManager.Repository.Data;
using TokenManager.Web.Api.Handlers.VOs;
using TokenManager.Web.Api.Models.Tokens;
using TokenManager.Web.Api.Services;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Handlers
{
    public class PendingTransactionsHandler : QueuedHandler
    {
        private readonly EosBlockchainService _eosBlockchainService;

        public PendingTransactionsHandler(ILogger<QueuedHandler> logger, EosBlockchainService eosBlockchainService) : base(logger)
        {
            _eosBlockchainService = eosBlockchainService;
        }

        public override async Task DoWork()
        {
            var comprasvendas = new List<PendingTransactionsVO>();
            using (var ctx = new GenbitDB_Context())
            {
               var query = "SELECT * FROM OfferBookPendingTransactions";
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
                                comprasvendas.Add(new PendingTransactionsVO
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
                                    UrlDestino = reader.GetString(10)
                                });
                            }
                        }
                    }
                }
            }

            var x = Parallel.ForEach(comprasvendas, new ParallelOptions { MaxDegreeOfParallelism = 1 }, async (compravenda) =>
            {
                Task.Delay(1000).Wait();
                await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(compravenda, Formatting.Indented));
                //var acc = await _eosBlockchainService.GetAccountAsync(compravenda.UrlOrigem);
                if (!_eosBlockchainService.AccountExists(compravenda.UrlDestino))
                    return;
                    //await _eosBlockchainService.NewAccountAsync(compravenda.UrlDestino, _eosBlockchainService.GetAccountAuthority());

                var result = await _eosBlockchainService.TransferAsync("euroexchange", compravenda.Simbolo, 4, compravenda.UrlOrigem, compravenda.UrlDestino, compravenda.Valor, compravenda.Unstaked, compravenda.Descricao);
                if (result.Item1)
                {
                    var transaction = await _eosBlockchainService.CheckTransactionAsync(result.Item2);
                    if (transaction)
                    {
                        var MoedaId = new SqlParameter("@MoedaId", compravenda.MoedaId);
                        var ContaId = new SqlParameter("@ContaId", compravenda.ContaId);
                        var Simbolo = new SqlParameter("@Simbolo", compravenda.Simbolo);
                        var LancamentoId = new SqlParameter("@LancamentoId", compravenda.LancamentoId);
                        var Tx = new SqlParameter("@Tx", result.Item2);
                        var Valor = new SqlParameter("@Valor", compravenda.Valor);
                        using var ctx = new GenbitDB_Context();
                        ctx.Database.ExecuteSqlRaw("INSERT INTO TransacaoToken ([Hash], DataCadastro, MoedaId, ContaId, Simbolo, LancamentoId, Tx, Valor, Taxa, Confirmado) VALUES(NEWID(), GETDATE(), @MoedaId, @ContaId, @Simbolo, @LancamentoId, @Tx, @Valor, 0, 0); ", MoedaId, ContaId, Simbolo, LancamentoId, Tx, Valor);
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"unknown transaction_id: {result.Item2}");
                    }
                }
                else
                {
                    await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(result, Formatting.Indented));
                }
            });
            while (!x.IsCompleted)
                await Task.CompletedTask;
        }
    }
}