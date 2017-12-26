using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    [ObjectEvent]
    public class UIRoomComponentEvent : ObjectEvent<UIRoomComponent>, IAwake, IStart
    {
        public void Awake()
        {
            this.Get().Awake();
        }

        public void Start()
        {
            this.Get().Start();
        }
    }

    public class UIRoomComponent : Component
    {
        private InteractionComponent interaction;

        private Text multiples;

        public readonly EQueue<GameObject> GamersPanel = new EQueue<GameObject>();

        public InteractionComponent Interaction
        {
            get
            {
                if (interaction == null)
                {
                    UI uiRoom = this.GetEntity<UI>();
                    UI uiInteraction = UIInteractionFactory.Create(Hotfix.Scene, UIType.Interaction, uiRoom);
                    interaction = uiInteraction.GetComponent<InteractionComponent>();
                    uiRoom.Add(uiInteraction);
                }
                return interaction;
            }
        }

        public void Awake()
        {
            ReferenceCollector rc = this.GetEntity<UI>().GameObject.GetComponent<ReferenceCollector>();

            GameObject quitButton = rc.Get<GameObject>("QuitButton");
            GameObject readyButton = rc.Get<GameObject>("ReadyButton");
            GameObject multiplesObj = rc.Get<GameObject>("Multiples");
            multiples = multiplesObj.GetComponent<Text>();
            //绑定事件
            quitButton.GetComponent<Button>().onClick.Add(OnQuit);
            readyButton.GetComponent<Button>().onClick.Add(OnReady);

            //默认隐藏UI
            multiplesObj.SetActive(false);
            readyButton.SetActive(false);
            rc.Get<GameObject>("Desk").SetActive(false);

            //添加玩家面板
            GameObject gamersPanel = rc.Get<GameObject>("Gamers");
            GamersPanel.Enqueue(gamersPanel.Get<GameObject>("Local"));
            GamersPanel.Enqueue(gamersPanel.Get<GameObject>("Left"));
            GamersPanel.Enqueue(gamersPanel.Get<GameObject>("Right"));
        }

        public void Start()
        {
            //添加本地玩家
            Player localPlayer = ClientComponent.Instance.LocalPlayer;
            Gamer localGamer = EntityFactory.CreateWithId<Gamer, long>(localPlayer.Id, localPlayer.UserID);
            localGamer.AddComponent<GamerUIComponent, UI>(this.GetEntity<UI>());
            GamerComponent gamerComponent = this.GetComponent<GamerComponent>();
            gamerComponent.Add(localGamer);
            gamerComponent.LocalGamer = localGamer;
        }

        /// <summary>
        /// 设置倍率
        /// </summary>
        /// <param name="multiples"></param>
        public void SetMultiples(int multiples)
        {
            this.multiples.gameObject.SetActive(true);
            this.multiples.text = multiples.ToString();
        }

        /// <summary>
        /// 重置倍率
        /// </summary>
        public void ResetMultiples()
        {
            this.multiples.gameObject.SetActive(false);
            this.multiples.text = "1";
        }

        /// <summary>
        /// 退出房间
        /// </summary>
        private void OnQuit()
        {
            //发送退出房间消息
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            SessionComponent.Instance.Session.Send(new Quit() { PlayerId = playerId });

            //切换到大厅界面
            Hotfix.Scene.GetComponent<UIComponent>().Create(UIType.UILobby);
            Hotfix.Scene.GetComponent<UIComponent>().Remove(UIType.UIRoom);
        }

        /// <summary>
        /// 准备
        /// </summary>
        private void OnReady()
        {
            //发送准备
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            SessionComponent.Instance.Session.Send(new PlayerReady() { PlayerId = playerId });
        }
    }
}
