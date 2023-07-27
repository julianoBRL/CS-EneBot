
using System.Threading.Tasks;
using Discord.Commands;

namespace EneBot.Modules{
    public class PrefixModule : ModuleBase<SocketCommandContext>{
        
        [Command("ping")]
        public async Task HandlePingCommand(){
            await Context.Message.Channel.SendMessageAsync("PONG!!");
        }

    }
}