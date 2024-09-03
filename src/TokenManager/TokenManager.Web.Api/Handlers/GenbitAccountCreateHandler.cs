using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TokenManager.Web.Api.Services;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Handlers
{
    public class GenbitAccountCreateHandler : QueuedHandler
    {
        private readonly EosBlockchainService _eosBlockchainService;
        public override int Delay { get; set; } = 60000;

        public GenbitAccountCreateHandler(ILogger<QueuedHandler> logger, EosBlockchainService eosBlockchainService) : base(logger)
        {
            _eosBlockchainService = eosBlockchainService;
        }

        public override async Task DoWork()
        {
            await Task.CompletedTask;
        }
    }
}