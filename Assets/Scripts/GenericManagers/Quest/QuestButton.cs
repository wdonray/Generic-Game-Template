using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    /// <summary>
    ///     Example Class
    /// </summary>
    public class QuestButton : MonoBehaviour
    {
        private QuestEvent.EventStatus _status;
        public Button ButtonComponent;

        public Color CurrentColor, WaitingColor, DoneColor;

        public Text EventName;

        public RawImage ImageColor;
        public QuestEvent ThisEvent;

        public void SetUp(QuestEvent e, GameObject scrollList)
        {
            ThisEvent = e;
            ButtonComponent.transform.SetParent(scrollList.transform, false);
            EventName.text = "<b>" + ThisEvent.Name + "</b>\n" + ThisEvent.Description;
            _status = ThisEvent.Status;
            ImageColor.color = WaitingColor;
            ButtonComponent.interactable = false;
        }

        public void UpdateButton(QuestEvent.EventStatus s)
        {
            _status = s;
            switch (_status)
            {
                case QuestEvent.EventStatus.Done:
                    ImageColor.color = DoneColor;
                    ButtonComponent.interactable = false;
                    break;
                case QuestEvent.EventStatus.Waiting:
                    ImageColor.color = WaitingColor;
                    ButtonComponent.interactable = false;
                    break;
                case QuestEvent.EventStatus.Current:
                    ImageColor.color = CurrentColor;
                    ButtonComponent.interactable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}