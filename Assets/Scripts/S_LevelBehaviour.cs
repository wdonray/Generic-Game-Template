using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

namespace GenericManagers
{
    public class S_LevelBehaviour : Singleton<S_LevelBehaviour>
    {
        VideoPlayer videoPlayer;
        public void Awake()
        {
            // Will attach a VideoPlayer to the main camera.
            GameObject camera = GameObject.Find("Main Camera");

            // VideoPlayer automatically targets the camera backplane when it is added
            // to a camera object, no need to change videoPlayer.targetCamera.
            videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();


            // Play on awake defaults to true. Set it to false to avoid the url set
            // below to auto-start playback since we're in Start().
            //videoPlayer.playOnAwake = false;

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
            videoPlayer.url = videoFilePath;
            //pepares the video for playback
            videoPlayer.Prepare();
            //checks if video is ready for play then plays it
            if (videoPlayer.isPrepared)
                videoPlayer.Play();
        }

        /// <summary>
        /// restarts the active scene
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        public void FadeToScene()
        {
            //loads the fade out animation
            Resources.Load("C: /Users/regir/Documents/GitHub/Generic - Game - Template/Assets/Resources/FadeOut.anim");

            var image = GameObject.FindObjectOfType<Image>().gameObject;
            var targetScene = SceneManager.GetSceneByName("TestScene2");
            var l = LoadLevelAsync(targetScene.ToString());
            //moves image object to scene that will be loaded
            SceneManager.MoveGameObjectToScene(image, targetScene);
            //loads next scene in background;
            
            if (l.isDone)
            {
                l.allowSceneActivation = true;
                Resources.Load("C: /Users/regir/Documents/GitHub/Generic - Game - Template/Assets/Resources/FadeIn.anim");

            }

        }
        

    }
}