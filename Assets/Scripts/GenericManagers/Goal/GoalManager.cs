using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Intertia.Goal
{
    public class GoalManager : MonoBehaviour
    {
        public Goal ActiveGoal;
        private DisplayGoal _displayGoal;
        public List<Goal> Goals = new List<Goal>();
        private Stack<Goal> _goalHolder = new Stack<Goal>();
        private bool _error;

        // Start is called before the first frame update
        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
            _displayGoal = FindObjectOfType<DisplayGoal>();
            if (Goals.Count != Goals.Distinct().Count())
            {
                Debug.LogError("Cannot have duplicated goals");
            }
            else
            {
                PopulateStack(Goals);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Raise();
            }
        }

        /// <summary>
        ///     Jump to next goal in the stack
        /// </summary>
        public void Raise()
        {
            if (_error)
            {
                print("Cannot raise due to error message");
            }
            else
            {
                if (_goalHolder.Count == 0)
                {
                    ActiveGoal = null;
                    print("Goal Stack is empty");
                }
                else
                {
                    var instance = _goalHolder.Peek();
                    if (instance.GetStatus() == GoalStatus.Current)
                    {
                        instance.Action.Invoke();
                        instance.UpdateStatus(GoalStatus.Done);
                        _displayGoal.UpdateList(instance);
                        _goalHolder.Pop();
                    }

                    if (_goalHolder.Count == 0)
                    {
                        ActiveGoal = null;
                        print("Goal Stack is empty");
                    }
                    else
                    {
                        instance = _goalHolder.Peek();
                        instance.UpdateStatus(GoalStatus.Current);
                        ActiveGoal = instance;
                        _displayGoal.UpdateList(ActiveGoal);
                    }
                }
            }
        }


        /// <summary>
        ///     Adds a goal to the stack
        /// </summary>
        /// <param name="goal"></param>
        public void AddToGoals(Goal goal)
        {
            _goalHolder.Push(goal);
        }

        /// <summary>
        ///     Clear all goals (Stack and List)
        /// </summary>
        public void ClearGoals()
        {
            Goals.Clear();
            _goalHolder.Clear();
        }

        /// <summary>
        ///     Update the Stack with what is in the list
        /// </summary>
        public void PopulateStack<T>(List<T> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                AddToGoals(Goals[i]);
                Goals[i].UpdateStatus(GoalStatus.Waiting);
            }

            _displayGoal.Setup();
            Raise();
        }

        void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
            foreach (var goal in Goals)
            {
                goal.UpdateStatus(GoalStatus.Waiting);
            }
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Error)
            {
                _error = true;
            }
        }
    }
}