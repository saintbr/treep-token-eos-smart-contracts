using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TokenManager.Web.Api.Models.Response.EOSIO;
using TokenManager.Web.Api.Services;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Handlers
{
    public class AccountHealthHandler : QueuedHandler
    {
        private readonly EosBlockchainService _eosBlockchainService;
        private string lower_bound = string.Empty;

        public AccountHealthHandler(ILogger<QueuedHandler> logger, EosBlockchainService eosBlockchainService) : base(logger) 
        {
            _eosBlockchainService = eosBlockchainService;
        }

        public override async Task DoWork()
        {
            var infos = await _eosBlockchainService.GetTableDataAsync<EOSIODelBandInfo>("eosio", "delband", "treepaccount", lower_bound);
            lower_bound = infos.LastOrDefault().to;

            Parallel.ForEach(infos, new ParallelOptions { MaxDegreeOfParallelism = 1 }, async (info) =>
            {
                var except = new[] { "treepaccount", "genbitbrasil", "treeppvantbr", "treepstaked1", "treepstaked2", "treepstaked3" };
                if (!except.Contains(info.to)) 
                {
                    Task.Delay(2500).Wait();
                    await _eosBlockchainService.AdjustAccountHealth(info.from, info.to, info.net_weight, info.cpu_weight);
                    await System.Console.Out.WriteLineAsync(JsonConvert.SerializeObject(new { info }, Formatting.Indented));

                    if (info.cpu_weight.StartsWith("0.3"))
                    {
                    }
                }                
            });
            await Task.CompletedTask;
        }
    }
}