using CommandLine;

namespace BetapetBotCaller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private BetapetBot bot;
        private Random random;

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

            random = new Random();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Load();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (GetBotIsAwake(DateTime.Now))
                {
                    try
                    {
                        bool handleResult = await bot.HandleEverything();

                        _logger.LogInformation("Handled betapetbot at {0}. Handle sucess: " + handleResult, DateTime.Now);
                    }
                    catch
                    {
                        _logger.LogWarning("Error when handling betapetbot");
                    }
                }
                else
                {

                }

                await Task.Delay((int)(1000 * 60 * GetSleepMinutes(DateTime.Now)), stoppingToken);
            }
        }

        private bool GetBotIsAwake(DateTime time)
        {
            double hour = time.Hour + (time.Minute / 60.0);
            double probability = Math.Sin((hour / (2 * Math.PI)) + (Math.PI * -0.3));
            return random.NextDouble() < probability;
        }

        private double GetSleepMinutes(DateTime time)
        {
            double hour = time.Hour + (time.Minute / 60.0);
            double sleepMinutes = (10 - (5 * Math.Sin((hour / (2 * Math.PI)) + (Math.PI * -0.3))) - Math.Sin(hour)) / 2.0;
            return sleepMinutes;
        }
    }
}