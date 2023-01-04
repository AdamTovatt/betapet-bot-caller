using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using System.Threading.Tasks;

namespace BetapetBotCaller
{
    internal class BotConfiguration
    {
        [Option('u', "username", Required = true, HelpText = "Set the username to use for the bot account")]
        public string Username { get; set; }

        [Option('p', "password", Required = true, HelpText = "Set the password to use for the bot account")]
        public string Password { get; set; }

        [Option("port", Required = false, HelpText = "The port at which the bot api is hosted")]
        public string Port { get; set; }
    }
}
