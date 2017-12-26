using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Model
{
    public class HandCardSprite : MonoBehaviour
    {
        public Card Poker { get; set; }
        private bool isSelect;

        void Start()
        {
            EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback = new EventTrigger.TriggerEvent();
            clickEntry.callback.AddListener(new UnityAction<BaseEventData>(OnClick));
            eventTrigger.triggers.Add(clickEntry);
        }

        public void OnClick(BaseEventData data)
        {
            float move = 50.0f;
            if (isSelect)
            {
                move = -move;
                Game.Scene.GetComponent<EventComponent>().Run<Card>(EventIdType.CancelHandCard, Poker);
            }
            else
            {
                Game.Scene.GetComponent<EventComponent>().Run<Card>(EventIdType.SelectHandCard, Poker);
            }
            RectTransform rectTransform = this.GetComponent<RectTransform>();
            rectTransform.anchoredPosition += Vector2.up * move;
            isSelect = !isSelect;
        }
    }
}
