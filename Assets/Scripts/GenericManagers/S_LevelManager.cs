﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GenericManagers
{
    /// <summary>
    ///     Created by: Reginald Reed 2018
    /// </summary>
    public class S_LevelManager : Singleton<S_LevelManager>
    {
        private VideoPlayer _videoPlayer;

        public void Awake()
        {
            // Will attach a VideoPlayer to the main camera.
            var cam = GameObject.Find("Main Camera");

            // VideoPlayer automatically targets the camera backplane when it is added
            // to a camera object, no need to change videoPlayer.targetCamera.
            _videoPlayer = cam.AddComponent<VideoPlayer>();

            // Play on awake defaults to true. Set it to false to avoid the url set
            // below to auto-start playback since we're in Start().
            //videoPlayer.playOnAwake = false;

            // By default, VideoPlayers added to a camera will use the far plane.
            // Let's target the near plane instead.
            _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            _videoPlayer.SetDirectAudioVolume(0, .2f);
        }

        /// <summary>
        ///     pauses the scene
        /// </summary>
        public void PauseScene()
        {
            Time.timeScale = 0;
        }

        /// <summary>
        ///     plays the scene at normal speed
        /// </summary>
        public void PlayScene()
        {
            Time.timeScale = 1;
        }

        /// <summary>
        ///     use to place scene in slow motion at any speed between 0 and 1
        /// </summary>
        /// <param name="sceneSpeed"></param>
        public void SlowMoScene(float sceneSpeed)
        {
            Time.timeScale = sceneSpeed;
        }

        /// <summary>
        ///     loads scene in the background of current scene
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
        ///     play video/cutscene. Takes in the file path to video
        /// </summary>
        /// <param name="videoFilePath"></param>
        public void PlayCutScene(string videoFilePath)
        {
            _videoPlayer.url = videoFilePath;
            //pepares the video for playback
            _videoPlayer.Prepare();
            //checks if video is ready for play then plays it
            if (_videoPlayer.isPrepared)
                _videoPlayer.Play();
        }

        /// <summary>
        ///     restarts the active scene
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        public void FadeToScene()
        {
            //loads the fade out animation
            Resources.Load("C: /Users/regir/Documents/GitHub/Generic - Game - Template/Assets/Resources/FadeOut.anim");

            var image = FindObjectOfType<Image>().gameObject;
            var targetScene = SceneManager.GetSceneByName("TestScene2");
            var l = LoadLevelAsync(targetScene.ToString());
            //moves image object to scene that will be loaded
            SceneManager.MoveGameObjectToScene(image, targetScene);
            //loads next scene in background;

            if (l.isDone)
            {
                l.allowSceneActivation = true;
                Resources.Load(
                    "C: /Users/regir/Documents/GitHub/Generic - Game - Template/Assets/Resources/FadeIn.anim");
            }
        }
    }
}