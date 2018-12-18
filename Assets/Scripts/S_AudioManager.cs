using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_AudioManager : Singleton<S_AudioManager>
{
    public AudioSource MainSource;

    public void OnEnable()
    {
        if (MainSource == null)
        {
            MainSource = gameObject.AddComponent<AudioSource>();
        }
    }
}
