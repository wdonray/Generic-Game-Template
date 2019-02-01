using System.Collections.Generic;
using System.Linq;
using Intertia.GenericManagers;
using UnityEngine;
using UnityEngine.UI;

namespace Intertia.Goal
{
    public class DisplayGoal : MonoBehaviour
    {
        public GameObject ViewPrefab;
        public Canvas MainCanvas;
        private GoalManager _goalManager;
        public List<GameObject> Views = new List<GameObject>();

        /// <summary>
        ///     Create a button for each goal
        /// </summary>
        public void Setup()
        {
            _goalManager = FindObjectOfType<GoalManager>();
            S_UIManager.Instance.ShowUI(MainCanvas, "Goal List", false, true);

            var goalList = S_UIManager.Instance.GetUI(MainCanvas, "Goal List");
            foreach (var goal in _goalManager.Goals)
            {
                var instance = Instantiate(ViewPrefab, goalList.GetChild(0).GetChild(0));
                instance.transform.GetChild(0).GetComponent<Text>().text = goal.Name;
                instance.transform.GetChild(1).GetComponent<Text>().text = goal.Description;
                instance.transform.GetChild(2).name = goal.GetStatus().ToString();
                Views.Add(instance);
            }
        }

        /// <summary>
        ///     Updates each button to the correct status
        /// </summary>
        public void UpdateList(Goal goal)
        {
            var instance = Views.SingleOrDefault(x => x.transform.GetChild(0).GetComponent<Text>().text == goal.Name);
            var instanceStatus = instance?.transform.GetChild(2);

            if (instanceStatus != null)
            {
                instanceStatus.name = goal.GetStatus().ToString();
                instance.GetComponent<Button>().interactable = instanceStatus.name == GoalStatus.Current.ToString();
            }
        }
    }
}