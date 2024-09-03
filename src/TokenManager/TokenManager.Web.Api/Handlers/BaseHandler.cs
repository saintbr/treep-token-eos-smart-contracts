using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace TokenManager.Web.Api.Handlers
{
    public class BaseHandler : BackgroundService, IHostedService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}