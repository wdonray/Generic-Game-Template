namespace QuestSystem
{
    public class QuestPath
    {
        public QuestEvent StartEvent, EndEvent;

        public QuestPath(QuestEvent from, QuestEvent to)
        {
            StartEvent = from;
            EndEvent = to;
        }
    }
}