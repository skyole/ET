using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    [ObjectEvent]
    public class GamerUIComponentEvent : ObjectEvent<GamerUIComponent>, IAwake<UI>
    {
        public void Awake(UI uiRoom)
        {
            this.Get().Awake(uiRoom);
        }
    }

    public class GamerUIComponent : Component
    {
        public GameObject Panel { get; private set; }
        public string NickName { get { return name.text; } }
        private Image headPhoto;
        private Text prompt;
        private Text name;
        private Text money;

        public void Awake(UI uiRoom)
        {
            //添加玩家UI
            this.Panel = uiRoom.GetComponent<UIRoomComponent>().GamersPanel.Dequeue();
            //绑定关联
            prompt = this.Panel.Get<GameObject>("Prompt").GetComponent<Text>();
            name = this.Panel.Get<GameObject>("Name").GetComponent<Text>();
            money = this.Panel.Get<GameObject>("Money").GetComponent<Text>();
            headPhoto = this.Panel.Get<GameObject>("HeadPhoto").GetComponent<Image>();

            UpdateInfo();

            if (this.GetEntity<Gamer>().IsReady)
            {
                SetReady();
            }
        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        public void InitPanel()
        {
            headPhoto.gameObject.SetActive(false);
            ResetPrompt();
            name.text = "空位";
            money.text = "";
        }

        /// <summary>
        /// 更新玩家信息
        /// </summary>
        public void UpdateInfo()
        {
            SetUserInfo();
            headPhoto.gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置玩家身份
        /// </summary>
        /// <param name="identity"></param>
        public void SetIdentity(Identity identity)
        {
            if (identity == Identity.None)
                return;

            string spriteName = $"Identity_{Enum.GetName(typeof(Identity), identity)}";
            Sprite headSprite = Resources.Load<GameObject>("UI").Get<GameObject>("Atlas").Get<Sprite>(spriteName);
            headPhoto.sprite = headSprite;
            headPhoto.gameObject.SetActive(true);
        }

        /// <summary>
        /// 玩家准备
        /// </summary>
        public void SetReady()
        {
            prompt.text = "准备！";
        }

        /// <summary>
        /// 出牌错误
        /// </summary>
        public void SetPlayCardsError()
        {
            prompt.text = "您出的牌不符合规则！";
        }

        /// <summary>
        /// 玩家不出
        /// </summary>
        public void SetDiscard()
        {
            prompt.text = "不出";
        }

        /// <summary>
        /// 玩家抢地主
        /// </summary>
        public void SetGrab(bool isGrab)
        {
            if (isGrab)
            {
                prompt.text = "抢地主";
            }
            else
            {
                prompt.text = "不抢";
            }
        }

        /// <summary>
        /// 重置提示
        /// </summary>
        public void ResetPrompt()
        {
            prompt.text = "";
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        public void GameStart()
        {
            ResetPrompt();
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="id"></param>
        private async void SetUserInfo()
        {
            GetUserInfoRe getUserInfoRE = await SessionComponent.Instance.Session.Call<GetUserInfoRe>(new GetUserInfoRt() { UserId = this.GetEntity<Gamer>().UserID });

            if (getUserInfoRE.Error == ErrorCode.ERR_QueryUserInfoError)
            {
                return;
            }

            name.text = getUserInfoRE.NickName;
            money.text = getUserInfoRE.Money.ToString();
        }

        public override void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            base.Dispose();
            //回收玩家UI
            InitPanel();
            UI room = Hotfix.Scene.GetComponent<UIComponent>()?.Get(UIType.UIRoom);
            room?.GetComponent<UIRoomComponent>().GamersPanel.Enqueue(Panel);
        }
    }
}
