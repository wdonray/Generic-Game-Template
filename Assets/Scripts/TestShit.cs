using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenericManagers.S_LevelBehaviour.Instance.PlayCutScene("file://C:/Users/regir/Documents/GitHub/Generic-Game-Template/Assets/Audio/When the music video doesn't match the song.mp4");
        //GenericManagers.S_LevelBehaviour.Instance.FadeToScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
