using UnityEngine;
using UnityEngine.UI;
using Model;
using System;

namespace Hotfix
{
    [ObjectEvent]
    public class UIEndComponentEvent : ObjectEvent<UIEndComponent>, IAwake<bool>
    {
        public void Awake(bool isWin)
        {
            this.Get().Awake(isWin);
        }
    }

    public class UIEndComponent : Component
    {
        private GameObject contentPrefab;
        private GameObject gamerContent;

        public void Awake(bool isWin)
        {
            ReferenceCollector rc = this.GetEntity<UI>().GameObject.GetComponent<ReferenceCollector>();

            if (isWin)
            {
                rc.Get<GameObject>("Lose").SetActive(false);
            }
            else
            {
                rc.Get<GameObject>("Win").SetActive(false);
            }

            gamerContent = rc.Get<GameObject>("GamerContent");
            Button continueButton = rc.Get<GameObject>("ContinueButton").GetComponent<Button>();
            continueButton.onClick.Add(OnContinue);
            contentPrefab = Resources.Load<GameObject>("UI").Get<GameObject>("Content");
        }

        /// <summary>
        /// 创建玩家结算信息
        /// </summary>
        /// <returns></returns>
        public GameObject CreateGamerContent(Gamer gamer, Identity winnerIdentity, long baseScore, int multiples, long score)
        {
            GameObject newContent = UnityEngine.Object.Instantiate(contentPrefab);
            newContent.transform.SetParent(gamerContent.transform, false);

            Identity gamerIdentity = gamer.GetComponent<HandCardsComponent>().AccessIdentity;
            Sprite identitySprite = Resources.Load<GameObject>("UI").Get<GameObject>("Atlas").Get<Sprite>($"Identity_{Enum.GetName(typeof(Identity), gamerIdentity)}");
            newContent.Get<GameObject>("Identity").GetComponent<Image>().sprite = identitySprite;

            string nickName = gamer.GetComponent<GamerUIComponent>().NickName;
            Text nickNameText = newContent.Get<GameObject>("NickName").GetComponent<Text>();
            Text baseScoreText = newContent.Get<GameObject>("BaseScore").GetComponent<Text>();
            Text multiplesText = newContent.Get<GameObject>("Multiples").GetComponent<Text>();
            Text scoreText = newContent.Get<GameObject>("Score").GetComponent<Text>();
            nickNameText.text = nickName;
            baseScoreText.text = baseScore.ToString();
            multiplesText.text = multiples.ToString();
            scoreText.text = score.ToString();

            if (gamer.Id == this.Entity.Parent.GetComponent<GamerComponent>().LocalGamer.Id)
            {
                nickNameText.color = Color.red;
                baseScoreText.color = Color.red;
                multiplesText.color = Color.red;
                scoreText.color = Color.red;
            }

            return newContent;
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        private void OnContinue()
        {
            UI entity = this.GetEntity<UI>();
            UI parent = (UI)entity.Parent;
            parent.GameObject.Get<GameObject>("ReadyButton").SetActive(true);
            parent.Remove(entity.Name);
        }
    }
}
