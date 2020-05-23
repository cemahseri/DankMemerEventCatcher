using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using EventCatcherSelfbot;

namespace DankMemerEventCatcher
{
    internal static class Program
    {
        private static IEnumerable<string> ExpectedPrefixes { get; } = new List<string>
        {
            "Type",
            "typing"
        };

        private static readonly Random Random = new Random();

        private static DiscordClient _client;
        private static Configuration _configuration;

        private static async Task Main()
        {
            _configuration = ConfigurationManager.GetConfiguration();

            _client = new DiscordClient(new DiscordConfiguration
            {
                Token = _configuration.Token,
                TokenType = TokenType.User,
                LogLevel = LogLevel.Critical,
                UseInternalLogHandler = true
            });

            _client.MessageCreated += OnMessageCreated;

            await _client.ConnectAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }

        private static async Task OnMessageCreated(MessageCreateEventArgs e)
        {
            // If it's Dank Memer bot.
            if (!e.Author.IsBot || e.Author.Id != 270904126974590976)
            {
                return;
            }

            // If the message contains `.
            var firstIndex = e.Message.Content.IndexOf("`", StringComparison.Ordinal);
            if (firstIndex == -1)
            {
                return;
            }

            // If the message contains only 1 `.
            var lastIndex = e.Message.Content.LastIndexOf("`", StringComparison.Ordinal);
            if (firstIndex == lastIndex)
            {
                return;
            }

            // If somebody used "pls say" command.
            if (e.Message.Content[e.Message.Content.Length - 5] == '#' ||
                e.Message.Content.Contains("https://") && e.Message.Content.Contains("dankmemer"))
            {
                return;
            }

            // If it's for other people.
            if (e.Message.Content.StartsWith("**Work for") || // If it's "pls work".
                e.Message.Content.StartsWith("ahhhhh")) // If it's "pls fish".
            {
                return;
            }

            var lastSecondIndex = e.Message.Content.LastIndexOf("`", lastIndex - 1, StringComparison.Ordinal);
            if (!ShouldContinue(e.Message.Content, lastSecondIndex)) // If it doesn't start with "Type" or "typing".
            {
                return;
            }

            // If it's heist.
            if (e.Message.Content.Contains("bank robbery") && !_configuration.JoinHeist)
            {
                return;
            }

            // Developer of the bot has been put invisible character (Alt + 0173) to between every character.
            // So, when you try to copy-paste the specific message, it'll not accept it. Also because of this, you cannot parse the message.
            // So, remove non-ASCII characters with this Regex pattern.
            var messageToSend = Regex.Replace(e.Message.Content.Substring(lastSecondIndex + 1, lastIndex - lastSecondIndex - 1), @"[^\u0000-\u007F]+", "");

            // Try to make it look more natural.
            await e.Channel.TriggerTypingAsync().ConfigureAwait(false);

            await Task.Delay(Random.Next(_configuration.MinimumInterval, _configuration.MaximumInterval + 1)).ConfigureAwait(false);

            await e.Channel.SendMessageAsync(messageToSend).ConfigureAwait(false);
            Console.WriteLine("Replied in " + e.Guild.Name + " guild, #" + e.Channel.Name + " channel.");
        }

        private static bool ShouldContinue(string source, int lastSecondIndex)
        {
            return ExpectedPrefixes.Any(p => source.Substring(lastSecondIndex - 1 - p.Length, p.Length) == p);
        }
    }
}