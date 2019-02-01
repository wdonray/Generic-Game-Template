using UnityEngine;
using UnityEngine.Events;

namespace Intertia.Goal
{
    public enum GoalStatus
    {
        Waiting, //not yet completed but can't be worked
        Current, //the one the player should be trying to achieve
        Done //has been achieved
    }

    [CreateAssetMenu(menuName = "ScriptableObject/Create Goal")]
    public class Goal : ScriptableObject
    {
        public string Name;
        public string Description;
        public UnityEvent Action;
        [SerializeField] private GoalStatus _status;

        /// <summary>
        ///     Sets your goals status to the passed in param
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(GoalStatus status)
        {
            _status = status;
        }

        /// <summary>
        ///     Retruns the goals status
        /// </summary>
        /// <returns></returns>
        public GoalStatus GetStatus()
        {
            return _status;
        }
    }
}