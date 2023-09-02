using WorkerServiceDeleteDuplicateUsers;

namespace WorkerServiceDeleteDuplicateUsers
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

        }

        public override Task StartAsync(CancellationToken cancellationToken)

        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.DeleteAsync("https://localhost:7104/api/Users/DeleteDuplicateUser");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("API endpoint was triggered successfully. Status code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("API endpoint returned an error. Status code {StatusCode}", result.StatusCode);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}

