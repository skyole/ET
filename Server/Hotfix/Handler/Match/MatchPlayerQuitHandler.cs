using System.Threading.Tasks;
using Model;

namespace Hotfix
{
    [ActorMessageHandler(AppType.Match)]
    public class MatchPlayerQuitHandler : AMActorHandler<Matcher, PlayerQuitMacth>
    {
        protected override Task Run(Matcher entity, PlayerQuitMacth message)
        {
            //移除匹配队列
            if (entity.PlayerId == message.PlayerId)
            {
                Game.Scene.GetComponent<MatcherComponent>().Remove(entity.PlayerId);
                Log.Info($"玩家{message.PlayerId}退出被移除匹配队列");
            }

            return Task.CompletedTask;
        }
    }
}
