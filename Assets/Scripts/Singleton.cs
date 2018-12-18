using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //  Check to see if we're about to be destroyed
    private static bool _shuttingDown = false;

    private static T _instance;

    /// <summary>
    ///     Access singleton instance through this property
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.Log("<color=yellow> [Singleton] Instance " + typeof(T) + " already destroyed returning null </color>");
                return null;
            }

            if (_instance == null)
            {   
                // Search for exisiting instance
                _instance = (T)FindObjectOfType(typeof(T));

                //  Create a new instance if one does not exist
                if (_instance == null)
                {
                    var singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = "----- " + typeof(T) + " -----";
                    //  Make instance persistent
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private void OnApplicationQuit()
    {
        _shuttingDown = true;
    }

    private void OnDestroy()
    {
        _shuttingDown = true;
    }
}
