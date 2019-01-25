using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public Quest TestQuest = new Quest();
        public List<GameObject> QuestLocations = new List<GameObject>();

        public List<GameObject> Buttons = new List<GameObject>(); // Example

        public GameObject QuestPrint, ButtonPrefab;

        void OnEnable()
        {
            FindObjectsOfType<QuestLocation>().ToList().ForEach(x => QuestLocations.Add(x.gameObject));
        }

        // Testing 
        void Start()
        {
            //Create each event
            var a = TestQuest.AddQuestEvent("test a", "first");
            var b = TestQuest.AddQuestEvent("test b", "second");
            var c = TestQuest.AddQuestEvent("test c", "third");

            //Define the paths between the events
            TestQuest.AddPath(a.GetId(), b.GetId());
            TestQuest.AddPath(b.GetId(), c.GetId());

            TestQuest.BreathFirstSearch(a.GetId());

            Buttons.Add(CreateButton(a).GetComponent<QuestButton>().gameObject);
            Buttons.Add(CreateButton(b).GetComponent<QuestButton>().gameObject);
            Buttons.Add(CreateButton(c).GetComponent<QuestButton>().gameObject);

            for (int i = 0; i < QuestLocations.Count; i++)
            {
                QuestLocations[i].GetComponent<QuestLocation>().Setup(this, Buttons[i].GetComponent<QuestButton>().ThisEvent, Buttons[i].GetComponent<QuestButton>());
                QuestLocations[i].name = Buttons[i].GetComponent<QuestButton>().ThisEvent.Name;
            }

            TestQuest.PrintPath();
        }

        void Update()
        {
            for (int i = 0; i < QuestLocations.Count; i++)
            {
                QuestLocations[i].GetComponent<Renderer>().material.color =
                    Buttons[i].GetComponent<QuestButton>().ImageColor.color;
            }
        }

        /// <summary>
        ///     Example function
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private GameObject CreateButton(QuestEvent e)
        {
            var b = Instantiate(ButtonPrefab);
            b.GetComponent<QuestButton>().SetUp(e, QuestPrint);

            if (e.Order == 1)
            {
                b.GetComponent<QuestButton>().UpdateButton(QuestEvent.EventStatus.Current);
                e.Status = QuestEvent.EventStatus.Current;
            }

            return b;
        }

        /// <summary>
        ///     if this event is the next in order
        ///     make the next in line available for completion
        /// </summary>
        /// <param name="e"></param>
        public void UpdateQuestsOnCompletion(QuestEvent e)
        {
            foreach (var questEvent in TestQuest.QuestEvents)
            {
                if (questEvent.Order == e.Order + 1)
                {
                    questEvent.UpdateQuestEvent(QuestEvent.EventStatus.Current);
                }
            }
        }
    }
}