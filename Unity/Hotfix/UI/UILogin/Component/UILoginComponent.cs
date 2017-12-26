using System;
using System.Net;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    [ObjectEvent]
    public class UILoginComponentEvent : ObjectEvent<UILoginComponent>, IAwake
    {
        public void Awake()
        {
            this.Get().Awake();
        }
    }

    public class UILoginComponent : Component
    {
        private InputField account;
        private InputField password;
        private Text prompt;
        private bool isLogining;
        private bool isRegistering;

        public void Awake()
        {
            ReferenceCollector rc = this.GetEntity<UI>().GameObject.GetComponent<ReferenceCollector>();

            //热更测试
            Text hotfixPrompt = rc.Get<GameObject>("HotfixPrompt").GetComponent<Text>();
            hotfixPrompt.text = "热更版本1.6";

            //绑定关联对象
            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();

            //添加事件
            rc.Get<GameObject>("LoginButton").GetComponent<Button>().onClick.Add(OnLogin);
            rc.Get<GameObject>("RegisterButton").GetComponent<Button>().onClick.Add(OnRegister);
        }

        /// <summary>
        /// 登录
        /// </summary>
        public async void OnLogin()
        {
            if (isLogining || this.Id == 0)
            {
                return;
            }

            isLogining = true;
            Session session = null;

            try
            {
                //创建登录服务器连接
                IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);
                session = Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);

                //发送登录请求
                prompt.text = "正在登录中....";
                LoginRe loginRE = await session.Call<LoginRe>(new LoginRt() { Account = account.text, Password = password.text });

                if (this.Id == 0)
                {
                    isLogining = false;
                    return;
                }

                prompt.text = "";
                if (loginRE.Error == ErrorCode.ERR_AccountOrPasswordError)
                {
                    prompt.text = "登录失败,账号或密码错误";
                    password.text = "";
                    session.Dispose();
                    isLogining = false;
                    return;
                }
                else if (loginRE.Error == ErrorCode.ERR_AccountOnline)
                {
                    prompt.text = "登录失败，账号已被登录";
                    password.text = "";
                    session.Dispose();
                    isLogining = false;
                    return;
                }

                //创建Gate服务器连接
                connetEndPoint = NetworkHelper.ToIPEndPoint(loginRE.Address);
                Session gateSession = Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
                Game.Scene.AddComponent<SessionComponent>().Session = gateSession;

                //登录Gate服务器
                LoginGateRe loginGateRE = await gateSession.Call<LoginGateRe>(new LoginGateRt() { Key = loginRE.Key });
                if (loginGateRE.Error == ErrorCode.ERR_ConnectGateKeyError)
                {
                    prompt.text = "登录网关服务器超时";
                    password.text = "";
                    session.Dispose();
                    isLogining = false;
                    return;
                }

                //保存连接,之后所有请求将通过这个连接发送
                SessionComponent sessionComponent = Game.Scene.AddComponent<SessionComponent>();
                sessionComponent.Session = gateSession;
                Log.Info("登录成功");

                //保存本地玩家
                Player player = Model.EntityFactory.CreateWithId<Player>(loginGateRE.PlayerId);
                player.UserID = loginGateRE.UserId;
                ClientComponent.Instance.LocalPlayer = player;

                //跳转到大厅界面
                Hotfix.Scene.GetComponent<UIComponent>().Create(UIType.UILobby);
                Hotfix.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);

            }
            catch (Exception e)
            {
                Log.Error(e.ToStr());
            }
            finally
            {
                session?.Dispose();
                isLogining = false;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        public async void OnRegister()
        {
            if (isRegistering)
            {
                return;
            }

            isRegistering = true;
            Session session = null;
            prompt.text = "";
            try
            {
                //创建登录服务器连接
                IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);
                session = Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);

                //发送注册请求
                prompt.text = "正在注册中....";
                RegisterRe registerRE = await session.Call<RegisterRe>(new RegisterRt() { Account = account.text, Password = password.text });
                prompt.text = "";
                if (registerRE.Error == ErrorCode.ERR_AccountAlreadyRegister)
                {
                    prompt.text = "注册失败，账号已被注册";
                    account.text = "";
                    password.text = "";
                    session.Dispose();
                    isRegistering = false;
                    return;
                }

                //注册成功自动登录
                OnLogin();
            }
            catch (Exception e)
            {
                Log.Error(e.ToStr());
            }
            finally
            {
                session?.Dispose();
                isRegistering = false;
            }
        }
    }
}
