using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Hotfix
{
    [ActorMessageHandler(AppType.DDZ)]
    public class GrabLordSelectHandler : AMActorHandler<Room, GrabLordSelect>
    {
        protected override Task Run(Room entity, GrabLordSelect message)
        {
            OrderControllerComponent orderController = entity.GetComponent<OrderControllerComponent>();
            GameControllerComponent gameController = entity.GetComponent<GameControllerComponent>();
            if (orderController.CurrentAuthority == message.PlayerId)
            {
                if (message.IsGrab)
                {
                    orderController.Biggest = message.PlayerId;
                    gameController.Multiples *= 2;
                    entity.Broadcast(new GameMultiples() { Multiples = gameController.Multiples });
                }

                //转发消息
                entity.Broadcast(message);

                if (orderController.SelectLordIndex >= entity.Count)
                {
                    /*
                     * 地主：√ 农民1：× 农民2：×
                     * 地主：× 农民1：√ 农民2：√
                     * 地主：√ 农民1：√ 农民2：√ 地主2：√
                     * 
                     * */
                    if (orderController.Biggest == 0)
                    {
                        //没人抢地主则重新发牌
                        gameController.BackToDeck();
                        gameController.DealCards();

                        //发送玩家手牌
                        Gamer[] gamers = entity.GetAll();
                        Dictionary<long, int> gamerCardsNum = new Dictionary<long, int>();
                        Array.ForEach(gamers, gamer => gamerCardsNum.Add(gamer.Id, gamer.GetComponent<HandCardsComponent>().GetAll().Length));
                        Array.ForEach(gamers, gamer =>
                        {
                            ActorProxy actorProxy = gamer.GetComponent<UnitGateComponent>().GetActorProxy();
                            actorProxy.Send(new GameStart()
                            {
                                GamerCards = gamer.GetComponent<HandCardsComponent>().GetAll(),
                                GamerCardsNum = gamerCardsNum
                            });
                        });

                        //随机先手玩家
                        gameController.RandomFirstAuthority();
                        return Task.CompletedTask;
                    }
                    else if ((orderController.SelectLordIndex == entity.Count &&
                        ((orderController.Biggest != orderController.FirstAuthority.Key && !orderController.FirstAuthority.Value) ||
                        orderController.Biggest == orderController.FirstAuthority.Key)) ||
                        orderController.SelectLordIndex > entity.Count)
                    {
                        gameController.CardsOnTable(orderController.Biggest);
                        return Task.CompletedTask;
                    }
                }

                //当所有玩家都抢地主时先手玩家还有一次抢地主的机会
                if (message.PlayerId == orderController.FirstAuthority.Key && message.IsGrab)
                {
                    orderController.FirstAuthority = new KeyValuePair<long, bool>(message.PlayerId, true);
                }

                orderController.Turn();
                orderController.SelectLordIndex++;
                entity.Broadcast(new SelectAuthority() { PlayerId = orderController.CurrentAuthority });
            }
            return Task.CompletedTask;
        }
    }
}
