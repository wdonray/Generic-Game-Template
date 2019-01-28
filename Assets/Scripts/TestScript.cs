using GenericManagers;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    //private TestClass _data;
    //private S_GameManager _gameManager;

    void Awake()
    {
        StartCoroutine(S_LevelManager.Instance.FadeScene(100, 5));
        //StartCoroutine(S_LevelManager.Instance.FlashScreen(0, 255, 5, 10));
        //_gameManager = S_GameManager.Instance;
        //_data = new TestClass
        //{
        //    IntData = 1,
        //    StringData = "One"
        //};
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    { 
    //        _gameManager.AddToEvent(_gameManager.OnSaveEvent, () => print("Save Event"));
    //        _gameManager.Serialize(_data, "/Donray.test");
    //    }

    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        _gameManager.AddToEvent(_gameManager.OnLoadEvent, () => print("Load Event"));
    //        var des = _gameManager.DeSerialize<TestClass>("/Donray.test");
    //        print("(TestClass) IntData: " + des.IntData);
    //        print("(TestClass) StringData: " + des.StringData);
    //    }
    //}
}
