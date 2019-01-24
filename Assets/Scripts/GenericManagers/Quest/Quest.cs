using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    //Path of Quest Events 
    public List<QuestEvent> QuestEvents = new List<QuestEvent>();

    public Quest() { }

    /// <summary>
    ///     Create an Event and add it to the event list
    /// </summary>
    /// <param name="n"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public QuestEvent AddQuestEvent(string n, string d)
    {
        var questEvent = new QuestEvent(n, d);
        QuestEvents.Add(questEvent);
        return questEvent;
    }

    /// <summary>
    ///     Takes two events and creates a path between them
    /// </summary>
    /// <param name="fromQuestEvent"></param>
    /// <param name="toQuestEvent"></param>
    public void AddPath(string fromQuestEvent, string toQuestEvent)
    {
        var from = FindQuestEvent(fromQuestEvent);
        var to = FindQuestEvent(toQuestEvent);

        if (from != null && to != null)
        {
            var p = new QuestPath(from, to);
            from.PathList.Add(p);
        }
    }

    /// <summary>
    ///     Returns a QuestEvent if it is found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private QuestEvent FindQuestEvent(string id)
    {
        foreach (var questEvent in QuestEvents)
        {
            if (questEvent.GetId() == id)
            {
                return questEvent;
            }
        }
        return null;
    }

    /// <summary>
    ///     Visits every single node
    ///     Reqursive algorithm, works way down through graph and updates ordernumber
    /// </summary>
    /// <param name="id"></param>
    /// <param name="orderNumber"></param>
    public void BreathFirstSearch(string id, int orderNumber = 1)
    {
        var thisEvent = FindQuestEvent(id);
        thisEvent.Order = orderNumber;

        foreach (var questPath in thisEvent.PathList)
        {
            if (questPath.EndEvent.Order == -1)
            {
                BreathFirstSearch(questPath.EndEvent.GetId(), orderNumber + 1);
            }
        }
    }

    /// <summary>
    ///     Prints the current Path
    /// </summary>
    public void PrintPath()
    {
        foreach (var questEvent in QuestEvents)
        {
            Debug.Log(questEvent.Name + " " + questEvent.Order);
        }
    }
}