using System;
using System.Threading.Tasks;
using Model;
using System.Collections.Generic;

namespace Hotfix
{
    [ActorMessageHandler(AppType.DDZ)]
    public class PlayCardsRtHandler : AMActorRpcHandler<Room, PlayCardsRt, PlayCardsRe>
    {
        protected override Task Run(Room unit, PlayCardsRt message, Action<PlayCardsRe> reply)
        {
            PlayCardsRe response = new PlayCardsRe();
            try
            {
                Gamer gamer = unit.Get(message.PlayerId);
                if (gamer == null)
                {
                    response.Error = ErrorCode.ErrPlayCardError;
                    reply(response);
                    return Task.CompletedTask;
                }

                GameControllerComponent gameController = unit.GetComponent<GameControllerComponent>();
                DeskCardsCacheComponent deskCardsCache = unit.GetComponent<DeskCardsCacheComponent>();
                OrderControllerComponent orderController = unit.GetComponent<OrderControllerComponent>();

                //检测是否符合出牌规则
                if (CardsHelper.PopEnable(message.Cards, out CardsType type))
                {
                    if (orderController.Biggest == orderController.CurrentAuthority ||
                        type == CardsType.JokerBoom ||
                        type == CardsType.Boom && CardsHelper.GetWeight(message.Cards, type) > deskCardsCache.GetTotalWeight() ||
                        (deskCardsCache.Rule == CardsType.Straight || deskCardsCache.Rule == CardsType.DoubleStraight || deskCardsCache.Rule == CardsType.TripleStraight) && type == deskCardsCache.Rule && message.Cards.Length == deskCardsCache.GetAll().Length && CardsHelper.GetWeight(message.Cards, type) > deskCardsCache.GetTotalWeight() ||
                        type == deskCardsCache.Rule && CardsHelper.GetWeight(message.Cards, type) > deskCardsCache.GetTotalWeight())
                    {
                        if (type == CardsType.JokerBoom)
                        {
                            gameController.Multiples *= 4;
                            unit.Broadcast(new GameMultiples() { Multiples = gameController.Multiples });
                        }
                        else if (type == CardsType.Boom)
                        {
                            gameController.Multiples *= 2;
                            unit.Broadcast(new GameMultiples() { Multiples = gameController.Multiples });
                        }
                    }
                    else
                    {
                        response.Error = ErrorCode.ErrPlayCardError;
                        reply(response);
                        return Task.CompletedTask;
                    }
                }
                else
                {
                    response.Error = ErrorCode.ErrPlayCardError;
                    reply(response);
                    return Task.CompletedTask;
                }

                //如果符合将牌从手牌移到出牌缓存区
                deskCardsCache.Clear();
                deskCardsCache.Rule = type;
                HandCardsComponent handCards = gamer.GetComponent<HandCardsComponent>();
                foreach (var card in message.Cards)
                {
                    handCards.PopCard(card);
                    deskCardsCache.AddCard(card);
                }

                //转发玩家出牌消息
                unit.Broadcast(new GamerPlayCards() { PlayerId = gamer.Id, Cards = message.Cards });

                if (handCards.CardsCount == 0)
                {
                    Identity winnerIdentity = unit.Get(orderController.Biggest).GetComponent<HandCardsComponent>().AccessIdentity;
                    Dictionary<long, long> gamersScore = new Dictionary<long, long>();

                    foreach (var _gamer in unit.GetAll())
                    {
                        _gamer.RemoveComponent<AutoPlayCardsComponent>();
                        gamersScore.Add(_gamer.Id, gameController.GetScore(_gamer, winnerIdentity));
                        //玩家剩余出牌
                        if (_gamer.Id != message.PlayerId)
                        {
                            Card[] _gamerCards = _gamer.GetComponent<HandCardsComponent>().GetAll();
                            unit.Broadcast(new GamerPlayCards() { PlayerId = _gamer.Id, Cards = _gamerCards });
                        }
                    }

                    //游戏结束结算
                    gameController.GameOver(gamersScore);

                    //广播游戏结束消息
                    unit.Broadcast(new Gameover()
                    {
                        Winner = winnerIdentity,
                        BasePointPerMatch = gameController.BasePointPerMatch,
                        Multiples = gameController.Multiples,
                        GamersScore = gamersScore
                    });
                }
                else
                {
                    //轮到下位玩家出牌
                    orderController.Biggest = gamer.Id;
                    orderController.Turn();
                    unit.Broadcast(new AuthorityPlayCard() { PlayerId = orderController.CurrentAuthority, IsFirst = false });
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
