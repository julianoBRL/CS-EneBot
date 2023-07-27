
/*
  Shintaro's girlfriend
*/

using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using EneBot.Handlers;
using EneBot.Modules;
using Microsoft.Extensions.Configuration;

//Need install to use (Already installed)
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EneBot{

  public class Program{

    public static Task Main() => new Program().MainAsync();

    private async Task MainAsync(){

      var config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("config.json");
      

      using IHost host = Host.CreateDefaultBuilder()
        .ConfigureServices(( action,services)=>
          services
            .AddSingleton(config)
            .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig{
              GatewayIntents = GatewayIntents.AllUnprivileged,
              AlwaysDownloadUsers = true
            }))
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .AddSingleton(x => new CommandService())
            .AddSingleton<PrefixHandler>()
            .AddSingleton(x => config.Build())
        ).Build();
      
      await RunAsync(host);

    }

    public async Task RunAsync(IHost host){
      using IServiceScope serviceScope = host.Services.CreateScope();
      IServiceProvider provider = serviceScope.ServiceProvider;

      var _client = provider.GetRequiredService<DiscordSocketClient>();
      var sCommands = provider.GetRequiredService<InteractionService>();
      await provider.GetRequiredService<InteractionHandler>().InitializeAsync();
      var config = provider.GetRequiredService<IConfigurationRoot>();

      var pCommands = provider.GetRequiredService<PrefixHandler>();
      pCommands.AddModule<PrefixModule>();
      pCommands.Initialize();

      _client.Log += async (LogMessage msg) => {Console.WriteLine(msg.Message);};
      sCommands.Log += async (LogMessage msg) => {Console.WriteLine(msg.Message);};

      _client.Ready += async () => {
        Console.WriteLine("Bom dia ^-^ <3");
        await sCommands.RegisterCommandsGloballyAsync();
      };

      await _client.LoginAsync(TokenType.Bot ,config.GetValue<string>("token"));
      await _client.StartAsync();

      await _client.SetGameAsync("o Shin na cama. <3 ");

      await Task.Delay(-1);
    }

  }
}