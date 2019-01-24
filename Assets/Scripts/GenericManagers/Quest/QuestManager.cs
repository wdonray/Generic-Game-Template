using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest TestQuest = new Quest();

    // Start is called before the first frame update
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

        TestQuest.PrintPath();
    }
}
