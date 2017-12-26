using UnityEngine;
using UnityEngine.UI;
using Model;
using System.Collections.Generic;

namespace Hotfix
{
    [ObjectEvent]
    public class InteractionComponentEvent : ObjectEvent<InteractionComponent>, IAwake
    {
        public void Awake()
        {
            this.Get().Awake();
        }
    }

    public class InteractionComponent : Component
    {
        private Button playButton;
        private Button promptButton;
        private Button discardButton;
        private Button grabButton;
        private Button disgrabButton;
        private Button changeGameModeButton;

        private List<Card> currentSelectCards = new List<Card>();

        private bool isAuto;

        public bool IsFirst { get; set; }

        public void Awake()
        {
            ReferenceCollector rc = this.GetEntity<UI>().GameObject.GetComponent<ReferenceCollector>();

            playButton = rc.Get<GameObject>("PlayButton").GetComponent<Button>();
            promptButton = rc.Get<GameObject>("PromptButton").GetComponent<Button>();
            discardButton = rc.Get<GameObject>("DiscardButton").GetComponent<Button>();
            grabButton = rc.Get<GameObject>("GrabButton").GetComponent<Button>();
            disgrabButton = rc.Get<GameObject>("DisgrabButton").GetComponent<Button>();
            changeGameModeButton = rc.Get<GameObject>("ChangeGameModeButton").GetComponent<Button>();

            //绑定事件
            playButton.onClick.Add(OnPlay);
            promptButton.onClick.Add(OnPrompt);
            discardButton.onClick.Add(OnDiscard);
            grabButton.onClick.Add(OnGrab);
            disgrabButton.onClick.Add(OnDisgrab);
            changeGameModeButton.onClick.Add(OnChangeGameMode);

            //默认隐藏UI
            playButton.gameObject.SetActive(false);
            promptButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
            grabButton.gameObject.SetActive(false);
            disgrabButton.gameObject.SetActive(false);
            changeGameModeButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public void Gameover()
        {
            changeGameModeButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void GameStart()
        {
            isAuto = false;
            changeGameModeButton.GetComponentInChildren<Text>().text = "自动";
            changeGameModeButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// 选中卡牌
        /// </summary>
        /// <param name="card"></param>
        public void SelectCard(Card card)
        {
            currentSelectCards.Add(card);
        }

        /// <summary>
        /// 取消选中卡牌
        /// </summary>
        /// <param name="card"></param>
        public void CancelCard(Card card)
        {
            currentSelectCards.Remove(card);
        }

        /// <summary>
        /// 清空选中卡牌
        /// </summary>
        public void Clear()
        {
            currentSelectCards.Clear();
        }

        /// <summary>
        /// 开始抢地主
        /// </summary>
        public void StartGrab()
        {
            grabButton.gameObject.SetActive(true);
            disgrabButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// 开始出牌
        /// </summary>
        public void StartPlay()
        {
            if (isAuto)
            {
                playButton.gameObject.SetActive(false);
                promptButton.gameObject.SetActive(false);
                discardButton.gameObject.SetActive(false);
            }
            else
            {
                playButton.gameObject.SetActive(true);
                promptButton.gameObject.SetActive(!IsFirst);
                discardButton.gameObject.SetActive(!IsFirst);
            }
        }

        /// <summary>
        /// 结束抢地主
        /// </summary>
        public void EndGrab()
        {
            grabButton.gameObject.SetActive(false);
            disgrabButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 结束出牌
        /// </summary>
        public void EndPlay()
        {
            playButton.gameObject.SetActive(false);
            promptButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 切换游戏模式
        /// </summary>
        private void OnChangeGameMode()
        {
            if (isAuto)
            {
                StartPlay();
                changeGameModeButton.GetComponentInChildren<Text>().text = "自动";
            }
            else
            {
                EndPlay();
                changeGameModeButton.GetComponentInChildren<Text>().text = "手动";
            }
            isAuto = !isAuto;
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            SessionComponent.Instance.Session.Send(new ChangeGameMode() { PlayerId = playerId });
        }

        /// <summary>
        /// 出牌
        /// </summary>
        private async void OnPlay()
        {
            CardHelper.Sort(currentSelectCards);
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            PlayCardsRe playCardsRE = await SessionComponent.Instance.Session.Call<PlayCardsRe>(new PlayCardsRt() { PlayerId = playerId, Cards = currentSelectCards.ToArray() });

            //出牌错误提示
            GamerUIComponent gamerUI = Hotfix.Scene.GetComponent<UIComponent>().Get(UIType.UIRoom).GetComponent<GamerComponent>().LocalGamer.GetComponent<GamerUIComponent>();
            if (playCardsRE.Error == ErrorCode.ERR_PlayCardError)
            {
                gamerUI.SetPlayCardsError();
            }
        }

        /// <summary>
        /// 提示
        /// </summary>
        private async void OnPrompt()
        {
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            PromptRe promptRE = await SessionComponent.Instance.Session.Call<PromptRe>(new PromptRt() { PlayerId = playerId });

            HandCardsComponent handCards = this.Entity.Parent.GetComponent<GamerComponent>().LocalGamer.GetComponent<HandCardsComponent>();

            //清空当前选中
            while (currentSelectCards.Count > 0)
            {
                Card selectCard = currentSelectCards[currentSelectCards.Count - 1];
                handCards.GetSprite(selectCard).GetComponent<HandCardSprite>().OnClick(null);
            }

            //自动选中提示出牌
            if (promptRE.Cards != null)
            {
                for (int i = 0; i < promptRE.Cards.Length; i++)
                {
                    handCards.GetSprite(promptRE.Cards[i]).GetComponent<HandCardSprite>().OnClick(null);
                }
            }
        }

        /// <summary>
        /// 不出
        /// </summary>
        private void OnDiscard()
        {
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            SessionComponent.Instance.Session.Send(new Discard() { PlayerId = playerId });
        }

        /// <summary>
        /// 抢地主
        /// </summary>
        private void OnGrab()
        {
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            SessionComponent.Instance.Session.Send(new GrabLordSelect() { PlayerId = playerId, IsGrab = true });
        }

        /// <summary>
        /// 不抢
        /// </summary>
        private void OnDisgrab()
        {
            long playerId = ClientComponent.Instance.LocalPlayer.Id;
            SessionComponent.Instance.Session.Send(new GrabLordSelect() { PlayerId = playerId, IsGrab = false });
        }
    }
}
