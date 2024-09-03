using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenManager.Repository.Data;
using TokenManager.Web.Api.Services.Stellar;

namespace TokenManager.Web.Api.Handlers
{
    public class StellarAccountCreatorHandler : QueuedHandler
    {
        private readonly StellarBlockchainService _stellarBlockchainService;

        public StellarAccountCreatorHandler(ILogger<QueuedHandler> logger, StellarBlockchainService stellarBlockchainService) : base(logger)
        {
            _stellarBlockchainService = stellarBlockchainService;
        }

        public override async Task DoWork()
        {
            List<long> contas = new List<long> { };
            using (var ctx = new GenbitDB_Context())
            {
                var query = "SELECT TOP 25 ContaId FROM TreepTokenPendingTransactions WHERE TipoTransacao = 'Crédito' AND ContaId NOT IN (SELECT ContaId FROM Carteira WHERE ContaId = TreepTokenPendingTransactions.ContaId AND MoedaId = 10)";
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
                                contas.Add(reader.GetInt64(0));
                            }
                        }
                    }
                }
            }

            foreach (var conta in contas)
            {
                var result = await _stellarBlockchainService.CreateAccount(conta);
                switch (result.Item1)
                {
                    case true:
                        {
                            var Url = new SqlParameter("@Url", result.Item2);
                            var ContaId = new SqlParameter("@ContaId", conta);
                            var Extra = new SqlParameter("@Extra", result.Item3);

                            using var ctx = new GenbitDB_Context();
                            ctx.Database.ExecuteSqlRaw("INSERT INTO Carteira (Descricao, Url, MoedaId, StatusCarteira, DataCadastro, ContaId, [Hash], DataConfirmacao, TipoCarteira, Extra) VALUES('Nova Carteira Treep', @Url, 10, 1, GETDATE(), @ContaId, NEWID(), GETDATE(), 1, @Extra)", Url, ContaId, Extra);
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