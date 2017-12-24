using System.Linq;
using System.Threading.Tasks;
using Model;
using System.Collections.Generic;
using System;

namespace Hotfix
{
    [ActorMessageHandler(AppType.DDZ)]
    public class PlayerReadyRtHandler : AMActorHandler<Room, PlayerReady>
    {
        protected override Task Run(Room entity, PlayerReady message)
        {
            Gamer gamer = entity.Get(message.PlayerId);

            if (gamer != null)
            {
                gamer.IsReady = true;

                Gamer[] gamers = entity.GetAll();

                //转发玩家准备消息
                entity.Broadcast(message);
                Log.Info($"玩家{gamer.Id}准备");

                //房间内有3名玩家且全部准备则开始游戏
                if (entity.Count == 3 && gamers.Where(g => g.IsReady).Count() == 3)
                {
                    //同步匹配服务器开始游戏
                    entity.State = RoomState.Game;
                    DDZHelper.SendMessage(new SyncRoomState() { RoomId = entity.Id, State = entity.State });

                    //初始玩家开始状态
                    foreach (var _gamer in gamers)
                    {
                        if (_gamer.GetComponent<HandCardsComponent>() == null)
                        {
                            _gamer.AddComponent<HandCardsComponent>();
                        }
                        _gamer.IsReady = false;
                    }

                    GameControllerComponent gameController = entity.GetComponent<GameControllerComponent>();
                    //洗牌发牌
                    gameController.DealCards();

                    Dictionary<long, int> gamerCardsNum = new Dictionary<long, int>();
                    Array.ForEach(gamers, (g) =>
                    {
                        HandCardsComponent handCards = g.GetComponent<HandCardsComponent>();
                        //重置玩家身份
                        handCards.AccessIdentity = Identity.None;
                        //记录玩家手牌数
                        gamerCardsNum.Add(g.Id, handCards.CardsCount);
                    });

                    //发送玩家手牌和其他玩家手牌数
                    foreach (var _gamer in gamers)
                    {
                        ActorProxy actorProxy = _gamer.GetComponent<UnitGateComponent>().GetActorProxy();
                        actorProxy.Send(new GameStart()
                        {
                            GamerCards = _gamer.GetComponent<HandCardsComponent>().GetAll(),
                            GamerCardsNum = gamerCardsNum
                        });
                    }

                    //随机先手玩家
                    gameController.RandomFirstAuthority();

                    Log.Info($"房间{entity.Id}开始游戏");
                }
            }

            return Task.CompletedTask;
        }
    }
}
