using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenManager.Repository.Data;
using TokenManager.Web.Api.Handlers.VOs;
using TokenManager.Web.Api.Services.Stellar;

namespace TokenManager.Web.Api.Handlers
{
    public class StellarAccountSignerHandler : QueuedHandler
    {
        private readonly StellarBlockchainService _stellarBlockchainService;

        public StellarAccountSignerHandler(ILogger<QueuedHandler> logger, StellarBlockchainService stellarBlockchainService) : base(logger)
        {
            _stellarBlockchainService = stellarBlockchainService;
        }

        public override async Task DoWork()
        {
            List<StellarAccountCheck> wallets = new List<StellarAccountCheck> { };
            using (var ctx = new GenbitDB_Context())
            {
                var query = "SELECT TOP 250 Url, Extra FROM Carteira WHERE MoedaId = 10 ORDER BY DataCadastro DESC";
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
                                wallets.Add(new StellarAccountCheck
                                {
                                    Account = reader.GetString(0),
                                    Seed = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            var tsks = Parallel.ForEach(wallets, new ParallelOptions { MaxDegreeOfParallelism = 1 }, async (wallet) =>
            {
                Task.Delay(2500).Wait();
                await _stellarBlockchainService.SignAccount(wallet.Account, wallet.Seed);
            });
            while (!tsks.IsCompleted)
                await Task.CompletedTask;
        }
    }
}