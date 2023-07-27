using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EneBot.Handlers{
    public class PrefixHandler{
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public PrefixHandler(DiscordSocketClient client, CommandService commands, IConfigurationRoot config){
            _client = client;
            _commands = commands;
            _config = config;
        }

        public void Initialize(){
            _client.MessageReceived += HandleCommandAsync;

        }

        public void AddModule<T>(){
            _commands.AddModuleAsync<T>(null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam){
            
            if (messageParam is not SocketUserMessage message) return;

            int argPos = 0;

            if (!(message.HasCharPrefix(_config.GetValue<char>("prefix"), ref argPos) || 
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);
            await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);

        }
    }
}