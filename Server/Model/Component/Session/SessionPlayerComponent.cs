using System.Net;

namespace Model
{
    [ObjectEvent]
    public class SessionPlayerComponentEvent : ObjectEvent<SessionPlayerComponent>, IAwake<Player>
    {
        public void Awake(Player player)
        {
            this.Get().Awake(player);
        }
    }


    public class SessionPlayerComponent : Component
    {
        public Player Player { get; private set; }

        public void Awake(Player player)
        {
            this.Player = player;
        }

        public override void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            base.Dispose();

            //向登录服务器发送玩家断开消息
            IPEndPoint realmAddress = Game.Scene.GetComponent<StartConfigComponent>().RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
            Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmAddress);
            realmSession.Send(new PlayerDisconnect() { UserId = Player.UserId });

            //如果在匹配中或游戏中发送退出消息
            if (Player.ActorId != 0)
            {
                ActorProxy actorProxy = Game.Scene.GetComponent<ActorProxyComponent>().Get(Player.ActorId);
                actorProxy.Send(new PlayerQuitDdz() { PlayerId = Player.Id });
            }

            Game.Scene.GetComponent<PlayerComponent>()?.Remove(this.Player.Id);
        }
    }
}