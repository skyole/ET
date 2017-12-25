using System.Threading.Tasks;
using Model;

namespace Hotfix
{
    [ActorMessageHandler(AppType.DDZ)]
    public class MapPlayerQuitHandler : AMActorHandler<Room, PlayerQuitDdz>
    {
        protected override Task Run(Room entity, PlayerQuitDdz message)
        {
            Gamer gamer = entity.Get(message.PlayerId);
            if (gamer != null)
            {
                if (entity.State == RoomState.Game)
                {
                    //玩家操作设置为自动
                    Log.Info($"游戏中，玩家{message.PlayerId}退出房间，切换为自动模式");
                    gamer.isOffline = true;
                    if (gamer.GetComponent<AutoPlayCardsComponent>() == null)
                    {
                        gamer.AddComponent<AutoPlayCardsComponent, Room>(entity);
                    }
                }
                else
                {
                    //房间移除玩家
                    entity.Remove(gamer.Id);

                    //同步匹配服务器移除玩家
                    DDZHelper.SendMessage(new GamerQuitRoom() { PlayerId = message.PlayerId, RoomId = entity.Id });

                    //消息广播给其他人
                    entity.Broadcast(new GamerOut() { PlayerId = message.PlayerId });
                    Log.Info($"准备中，玩家{message.PlayerId}退出房间");
                }
            }

            return Task.CompletedTask;
        }
    }
}
