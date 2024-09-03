using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenManager.Repository.Data;
using TokenManager.Web.Api.Handlers.VOs;
using TokenManager.Web.Api.Models.Response.API;
using TokenManager.Web.Api.Services;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Handlers
{
    public class SettlementHandler : QueuedHandler
    {
        private readonly EosBlockchainService _eosBlockchainService;

        public SettlementHandler(ILogger<QueuedHandler> logger, EosBlockchainService eosBlockchainService) : base(logger) 
        {
            _eosBlockchainService = eosBlockchainService;
        }

        public override async Task DoWork()
        {
            var diff = new List<Tuple<string, decimal, decimal>> { };
            var settlements = new List<SettlementTreepTokenVO>();
            using (var ctx = new GenbitDB_Context())
            {
                var query = "SELECT * FROM SettlementTreepToken";
                using (var command = ctx.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    ctx.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                settlements.Add(new SettlementTreepTokenVO
                                {
                                    ContaId = reader.GetInt64(0),
                                    Simbolo = reader.GetString(1),
                                    Carteira = reader.GetString(2),
                                    Saldo = reader.GetDecimal(3),
                                    Transacoes = reader.GetInt32(4)
                                });
                            }
                        }
                    }
                }
                Parallel.ForEach(settlements, new ParallelOptions { MaxDegreeOfParallelism = 1 }, async (settlement) =>
                {
                    await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(settlement, Formatting.Indented));
                    var balances = await _eosBlockchainService.GetBalanceAsync(settlement.Carteira, "euroexchange");
                    var balance = new AccountBalanceResponse(balances).FirstOrDefault(b => b.Symbol.Equals(settlement.Simbolo));
                    Task.Delay(100);
                    if (!balance.Balance.Equals(decimal.Round(settlement.Saldo, 4)))
                    {
                        diff.Add(new Tuple<string, decimal, decimal>(settlement.Carteira, balance.Balance, settlement.Saldo));
                    }
                    await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(balance, Formatting.Indented));
                });
            }
            Console.WriteLine(JsonConvert.SerializeObject(diff, Formatting.Indented));
            await Task.CompletedTask;
        }
    }
}