using System;
using System.Threading.Tasks;
using Model;

namespace Hotfix
{
    [ActorMessageHandler(AppType.DDZ)]
    public class PlayerJoinRoomRtHandler : AMActorRpcHandler<Room, PlayerJoinRoomRt, PlayerJoinRoomRe>
    {
        protected override Task Run(Room unit, PlayerJoinRoomRt message, Action<PlayerJoinRoomRe> reply)
        {
            PlayerJoinRoomRe response = new PlayerJoinRoomRe();

            try
            {
                Gamer gamer = unit.GetComponent<RoomJoinKeyComponent>().Get(message.Key);

                //验证密匙
                if (gamer != null)
                {
                    unit.Add(gamer);

                    //广播消息
                    Gamer[] gamers = unit.GetAll();
                    GamerInfo[] gamersInfo = new GamerInfo[gamers.Length];
                    for (int i = 0; i < gamers.Length; i++)
                    {
                        gamersInfo[i] = new GamerInfo();
                        gamersInfo[i].PlayerId = gamers[i].Id;
                        gamersInfo[i].UserId = gamers[i].UserId;
                        gamersInfo[i].IsReady = gamers[i].IsReady;
                    }
                    unit.Broadcast(new GamerEnter() { RoomId = unit.Id, GamersInfo = gamersInfo });
                    Log.Info($"玩家{gamer.Id}进入房间");
                }
                else
                {
                    Log.Info($"玩家进入房间验证失败，密匙：{message.Key}");
                    response.Error = ErrorCode.ErrJoinRoomError;
                }

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
            return Task.CompletedTask;
        }
    }
}
