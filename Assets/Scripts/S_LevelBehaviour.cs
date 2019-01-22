using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GenericManagers
{
    public class S_LevelBehaviour : Singleton<S_LevelBehaviour>
    {
        VideoPlayer videoPlayer;
        GameObject camera;
        Renderer screen;
        public void Awake()
        {
            // Will attach a VideoPlayer to the main camera.
            camera = GameObject.Find("Main Camera");
            screen = camera.transform.GetChild(0).GetComponent<Renderer>();
            // VideoPlayer automatically targets the camera backplane when it is added
            // to a camera object, no need to change videoPlayer.targetCamera.
            videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();


            // Play on awake defaults to true. Set it to false to avoid the url set
            // below to auto-start playback since we're in Start().
            videoPlayer.playOnAwake = false;

            // By default, VideoPlayers added to a camera will use the far plane.
            // Let's target the near plane instead.
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
            videoPlayer.SetDirectAudioVolume(0, .2f);
        }

        /// <summary>
        /// scene speed can go from 0 to 1. 
        /// <para>0 = pause scene</para>
        /// 1 = play scene at normal speed
        /// <para>
        /// </summary>
        /// <param name="speed"></param>
        public void ChangeSceneSpeed(float speed)
        {
            if (speed > 1)
                speed = 1;
            else if (speed < 0)
                speed = 0;
            Time.timeScale = speed;

        }

        /// <summary>
        /// loads scene in the background of current scene
        /// </summary>
        /// <param name="levelIndex"></param>
        public AsyncOperation LoadLevelAsync(int levelIndex)
        {
            return SceneManager.LoadSceneAsync(levelIndex);
        }

        public AsyncOperation LoadLevelAsync(string levelName)
        {
            return SceneManager.LoadSceneAsync(levelName);
        }

        /// <summary>
        /// play video/cutscene. Takes in the file path to video
        /// </summary>
        /// <param name="video"></param>
        public void PlayCutScene(string videoFilePath)
        {

            if (videoFilePath == null)
                Debug.LogWarning("file does not exist");

            videoPlayer.url = videoFilePath;

            //plays the video clip
            videoPlayer.Play();
        }

        /// <summary>
        /// restarts the active scene
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        public IEnumerator FadeOutScene()
        {
            while (true)
            {
                screen.material.color += new Color(0, 0, 0, .001f);
                while (screen.material.color.a >= 1)
                {
                    LoadLevelAsync("TestScene2");
                    StartCoroutine(FadeInScene());
                    yield return null;
                }
            }
        }
        
        public IEnumerator FadeInScene()
        {
            //StopCoroutine(FadeOutScene());

            var screen = camera.transform.GetChild(0).GetComponent<Renderer>();
            screen.material.color = new Color(0, 0, 0, 1);
            while (true)
            {
                screen.material.color += new Color(0, 0, 0, .001f);
                if(screen.material.color.a <= 0)
                    yield return null;
            }
            
        }
    }
}