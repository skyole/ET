using System;
using Model;

namespace Hotfix
{
    [MessageHandler(AppType.Match)]
    public class JoinMatchRtHandler : AMRpcHandler<JoinMatchRt, JoinMatchRe>
    {
        protected override async void Run(Session session, JoinMatchRt message, Action<JoinMatchRe> reply)
        {
            JoinMatchRe response = new JoinMatchRe();
            try
            {
                MatchComponent matchComponent = Game.Scene.GetComponent<MatchComponent>();
                if (matchComponent.Playing.ContainsKey(message.UserId))
                {
                    //重连房间
                    long roomId = matchComponent.Playing[message.UserId];
                    RoomManagerComponent roomManager = Game.Scene.GetComponent<RoomManagerComponent>();
                    Room room = roomManager.Get(roomId);
                    foreach (var gamer in room.GetAll())
                    {
                        if (gamer.UserId == message.UserId)
                        {
                            long pastId = gamer.Id;
                            gamer.Id = message.PlayerId;
                            room.Replace(pastId, gamer);
                            break;
                        }
                    }
                    ActorProxy actorProxy = Game.Scene.GetComponent<ActorProxyComponent>().Get(roomId);
                    actorProxy.Send(new PlayerReconnect() { PlayerId = message.PlayerId, UserId = message.UserId, GateSessionId = message.GateSessionId });

                    response.ActorId = roomId;
                    reply(response);
                    return;
                }

                //创建匹配玩家
                Matcher matcher = EntityFactory.Create<Matcher, long>(message.PlayerId);
                matcher.UserId = message.UserId;
                matcher.GateSessionId = message.GateSessionId;
                matcher.GateAppId = message.GateAppId;

                await matcher.AddComponent<ActorComponent>().AddLocation();
                //加入匹配队列
                Game.Scene.GetComponent<MatcherComponent>().Add(matcher);
                Log.Info($"玩家{message.PlayerId}加入匹配队列");

                response.ActorId = matcher.Id;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
