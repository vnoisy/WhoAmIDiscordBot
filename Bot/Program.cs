using Bot.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;


namespace Bot
{
    public class Program
    {
        private CommandService _commands;
        private DiscordSocketClient _client;
        private IServiceProvider _services;
        public static List<GameModel> game = new List<GameModel>();
        private static void Main() => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            // Avoid hard coding your token. Use an external source instead in your code.
            string token = null;

            _services = new ServiceCollection()
                .AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

            await InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommandAsync;

            _client.Ready += Client_Ready;

            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private Task Client_Ready()
        {
            /*foreach (var item in _client.Guilds)
            {
                Console.WriteLine(item.Name);

                Console.WriteLine("Text Channels");
                foreach (var text in item.TextChannels)
                {
                    Console.WriteLine("--" + text.Name);
                }

                Console.WriteLine("Voice Channels");
                foreach (var text in item.VoiceChannels)
                {
                    Console.WriteLine("--" + text.Name);
                }

                Console.WriteLine("Users");
                foreach (var text in item.Users)
                {
                    Console.WriteLine("--" + text.Username);
                }
            }*/

            return null;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new SocketCommandContext(_client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
