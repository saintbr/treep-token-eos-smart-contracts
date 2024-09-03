using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace TokenManager.Web.Api.Handlers
{
    public abstract class QueuedHandler : BaseHandler
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        protected readonly ILogger<QueuedHandler> _logger;
        public virtual int Delay { get; set; } = 10000;

        public QueuedHandler(ILogger<QueuedHandler> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            return Task.CompletedTask;
        }

        public abstract Task DoWork();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogInformation($" QueuedService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"QueuedService task doing background work.");
                await DoWork();
                await Task.Delay(Delay, stoppingToken);
            }
            _logger.LogInformation($"QueuedService background task is stopping.");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
                return;

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            }
        }

        public virtual new void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}