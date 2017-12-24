using System.Net;
using Model;

namespace Hotfix
{
    public static class DDZHelper
    {
        /// <summary>
        /// 发送消息给匹配服务器
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage(AMessage message)
        {
            IPEndPoint matchAddress = Game.Scene.GetComponent<StartConfigComponent>().MatchConfig.GetComponent<InnerConfig>().IPEndPoint;
            Session matchSession = Game.Scene.GetComponent<NetInnerComponent>().Get(matchAddress);
            matchSession.Send(message);
        }
    }
}
