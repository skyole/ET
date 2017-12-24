using System;
using System.Threading.Tasks;
using Model;

namespace Hotfix
{
    [ActorMessageHandler(AppType.DDZ)]
    public class GetJoinRoomKeyRtHandler : AMActorRpcHandler<Room, GetJoinRoomKeyRt, GetJoinRoomKeyRe>
    {
        protected override Task Run(Room unit, GetJoinRoomKeyRt message, Action<GetJoinRoomKeyRe> reply)
        {
            GetJoinRoomKeyRe response = new GetJoinRoomKeyRe();
            try
            {
                //创建玩家
                Gamer gamer = EntityFactory.CreateWithId<Gamer, long>(message.PlayerId, message.UserId);
                gamer.AddComponent<UnitGateComponent, long>(message.GateSeesionId);

                //随机密匙
                long key = RandomHelper.RandInt64();
                unit.GetComponent<RoomJoinKeyComponent>().Add(key, gamer);
                Log.Info($"获取进入房间密匙{key}");

                response.Key = key;
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
