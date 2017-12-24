using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using System.Linq;

namespace Hotfix
{
    [ActorMessageHandler(AppType.DDZ)]
    public class PromptRtHandler : AMActorRpcHandler<Room, PromptRt, PromptRe>
    {
        protected override async Task Run(Room unit, PromptRt message, Action<PromptRe> reply)
        {
            PromptRe response = new PromptRe();
            try
            {
                Gamer gamer = unit.Get(message.PlayerId);
                if (gamer != null)
                {
                    List<Card> handCards = new List<Card>(gamer.GetComponent<HandCardsComponent>().GetAll());
                    CardsHelper.SortCards(handCards);
                    if (gamer.Id == unit.GetComponent<OrderControllerComponent>().Biggest)
                    {
                        response.Cards = handCards.Where(card => card.CardWeight == handCards[handCards.Count - 1].CardWeight).ToArray();
                    }
                    else
                    {
                        DeskCardsCacheComponent deskCardsCache = unit.GetComponent<DeskCardsCacheComponent>();
                        List<Card[]> result = await CardsHelper.GetPrompt(handCards, deskCardsCache, deskCardsCache.Rule);

                        if (result.Count > 0)
                        {
                            response.Cards = result[RandomHelper.RandomNumber(0, result.Count)];
                        }
                    }
                }
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
