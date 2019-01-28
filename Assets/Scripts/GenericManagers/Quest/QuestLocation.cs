using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    /// <summary>
    ///     This code goes on a gameobject that represents the task to be performed
    ///     This script can contain any logic and should be changed per project.
    /// </summary>
    public class QuestLocation : MonoBehaviour
    {
        public string PlayerTag = "Player";
        private QuestManager _qManager;
        public QuestEvent QEvent;
        private QuestButton _qButton;

        private void Awake()
        {
            _qManager = FindObjectOfType<QuestManager>();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag != PlayerTag) return;

            //if we shouldn't be working on this event then don't register it as completed
            if (QEvent.Status != QuestEvent.EventStatus.Current) return;

            //Inject these back into the Quest Manager to Update Status
            QEvent.UpdateQuestEvent(QuestEvent.EventStatus.Done);
            //Example
            _qButton.UpdateButton(QuestEvent.EventStatus.Done);

            _qManager.UpdateQuestsOnCompletion(QEvent);
        }

        /// <summary>
        ///     Link up Questlocation to everything that is needed
        /// </summary>
        /// <param name="qm"></param>
        /// <param name="qe"></param>
        /// <param name="qb"></param>
        public void Setup(QuestManager qm, QuestEvent qe, QuestButton qb)
        {
            _qManager = qm;
            QEvent = qe;
            _qButton = qb;
            qe.Button = _qButton;
        }
    }
}