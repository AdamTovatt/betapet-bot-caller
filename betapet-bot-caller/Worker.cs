using CommandLine;

namespace BetapetBotCaller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private BetapetBot bot;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            Load();
        }

        private void HandleParse(BotConfiguration configuration)
        {
            bot = new BetapetBot(configuration.Username, configuration.Password, configuration.Port);
        }

        private void HandleParseError(IEnumerable<Error> errors)
        {
            foreach(Error error in errors)
            {
                _logger.LogError(error.ToString());
            }

            throw new ArgumentException("Username and/or password is missing as command line arguments!");
        }

        private void Load()
        {
            if (bot != null)
                return;

            Parser.Default.ParseArguments<BotConfiguration>(Environment.GetCommandLineArgs()).WithParsed(HandleParse).WithNotParsed(HandleParseError);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Load();

            while (!stoppingToken.IsCancellationRequested)
            {
                bool handleResult = await bot.HandleEverything();

                _logger.LogInformation("Handled betapetbot at {0}. Handle sucess: " + handleResult, DateTime.Now);

                await Task.Delay(1000 * 60 * 5, stoppingToken);
            }
        }
    }
}