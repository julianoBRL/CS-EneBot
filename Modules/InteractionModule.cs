
using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace EneBot.Modules{
    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>{
        
        [SlashCommand("ping", "Just a ping!")]
        public async Task HandlePingCommand(){
            await RespondAsync("PONG!!");
        }

        [SlashCommand("ene", "About me")]
        public async Task HandleEneCommand(){
            var d = Context.Client.CurrentUser.GetAvatarUrl(ImageFormat.Auto);

            var embedBuiler = new EmbedBuilder()
                .WithTitle("Ene")
                .WithDescription("Oi Oi\nSou a Ene, Bot que o Shin :heart: esta desenvolvendo!\nEspero me divertir com todos ^-^.")
                .WithColor(Color.Blue)
                .WithThumbnailUrl(d)
                .WithCurrentTimestamp();

            // Now, Let's respond with the embed.
            await RespondAsync(embed: embedBuiler.Build());
        }
        
        [SlashCommand("test", "não é da sua conta ^-^")]
        public async Task HandleEneCommand2(){
            if(Context.Interaction.User.Id != 229273685108850688){
                await RespondAsync("Comando de teste, vc não pode acessar isso!");
                return;
            }

            

        }
        

    }
}