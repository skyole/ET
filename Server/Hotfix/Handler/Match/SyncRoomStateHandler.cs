using Model;

namespace Hotfix
{
    [MessageHandler(AppType.Match)]
    public class SyncRoomStateHandler : AMHandler<SyncRoomState>
    {
        protected override void Run(Session session, SyncRoomState message)
        {
            RoomManagerComponent roomManager = Game.Scene.GetComponent<RoomManagerComponent>();
            Room room = roomManager.Get(message.RoomId);

            //同步房间状态
            switch (message.State)
            {
                case RoomState.Game:
                    roomManager.RoomStartGame(room.Id);
                    Log.Info($"房间{room.Id}切换为游戏状态");
                    break;
                case RoomState.Ready:
                    Log.Info($"房间{room.Id}切换为准备状态");
                    roomManager.RoomEndGame(room.Id);
                    break;
            }
        }
    }
}
