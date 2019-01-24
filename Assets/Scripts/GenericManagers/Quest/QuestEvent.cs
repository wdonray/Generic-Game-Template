using System;
using System.Collections.Generic;

public class QuestEvent
{
    public enum EventStatus
    {
        Waiting, //not yet completed but can't be worked
        Current, //the one the player should be trying to achieve
        Done //has been achieved
    }

    public string Name, Description, Id; //Information about the current QuestEvent

    public List<QuestPath> PathList = new List<QuestPath>(); //List of paths that can be done for the QuestEvent
    public EventStatus Status; //Current Status of the QuestEvent
    public int Order = -1; //Order in the path

    /// <summary>
    ///     Constructor for the QuestEvent
    /// </summary>
    /// <param name="n"></param>
    /// <param name="d"></param>
    public QuestEvent(string n, string d)
    {
        Name = n;
        Description = d;
        Id = Guid.NewGuid().ToString();
        Status = EventStatus.Waiting;
    }
    
    /// <summary>
    ///     Change QuestEvent status
    /// </summary>
    /// <param name="es"></param>
    public void UpdateQuestEvent(EventStatus es)
    {
        Status = es;
    }

    /// <summary>
    ///     Returns QuestEvent's Id
    /// </summary>
    /// <returns></returns>
    public string GetId()
    {
        return Id;
    }
}