using System;
using Model;
using System.Collections.Generic;

namespace Hotfix
{
    [MessageHandler(AppType.Realm)]
    public class LoginRtHandler : AMRpcHandler<LoginRt, LoginRe>
    {
        protected override async void Run(Session session, LoginRt message, Action<LoginRe> reply)
        {
            LoginRe response = new LoginRe();
            try
            {
                Log.Info($"Session请求登录 账号:{message.Account} 密码:{message.Password}");
                //数据库操作对象
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                //验证账号密码是否正确
                List<AccountInfo> result = await dbProxy.QueryJson<AccountInfo>($"{"{'Account':'"}{message.Account}{"','Password':'"}{message.Password}{"'}"}");
                if (result.Count == 0)
                {
                    response.Error = ErrorCode.ERR_AccountOrPasswordError;
                    reply(response);
                    return;
                }

                AccountInfo account = result[0];
                //验证账号是否在线
                if (Game.Scene.GetComponent<PlayerLoginManagerComponent>().Get(account.Id) != 0)
                {
                    response.Error = ErrorCode.ErrAccountOnline;
                    reply(response);
                    return;
                }

                //随机分配网关服务器
                StartConfig gateConfig = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();
                Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateConfig.GetComponent<InnerConfig>().IPEndPoint);

                //请求登录Gate服务器密匙
                GetLoginKeyRe getLoginKeyRE = await gateSession.Call<GetLoginKeyRe>(new GetLoginKeyRt() { UserId = account.Id });

                //添加账号在线标记
                Game.Scene.GetComponent<PlayerLoginManagerComponent>().Add(account.Id, gateConfig.AppId);

                response.Key = getLoginKeyRE.Key;
                response.Address = gateConfig.GetComponent<OuterConfig>().IPEndPoint.Address.ToString();
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
