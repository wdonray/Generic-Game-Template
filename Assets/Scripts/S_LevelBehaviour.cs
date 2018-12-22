﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace GenericManagers
{
    public class S_LevelBehaviour : Singleton<S_LevelBehaviour>
    {

        public void Start()
        {
            // Will attach a VideoPlayer to the main camera.
            GameObject camera = GameObject.Find("Main Camera");

            // VideoPlayer automatically targets the camera backplane when it is added
            // to a camera object, no need to change videoPlayer.targetCamera.
            var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();

            // Play on awake defaults to true. Set it to false to avoid the url set
            // below to auto-start playback since we're in Start().
            videoPlayer.playOnAwake = false;
        }

        /// <summary>
        ///  pauses the scene
        /// </summary>
        public void PauseScene()
        {
            Time.timeScale = 0;
        }

        /// <summary>
        /// plays the scene at normal speed
        /// </summary>
        public void PlayScene()
        {
            Time.timeScale = 1;
        }

        /// <summary>
        /// use to place scene in slow motion at any speed between 0 and 1
        /// </summary>
        /// <param name="sceneSpeed"></param>
        public void SlowMoScene(float sceneSpeed)
        {
            Time.timeScale = sceneSpeed;
        }

        /// <summary>
        /// loads scene in the background of current scene
        /// </summary>
        /// <param name="levelIndex"></param>
        public void LoadLevelAsync(int levelIndex)
        {
            SceneManager.LoadSceneAsync(levelIndex);
        }

        /// <summary>
        /// play video/cutscene
        /// </summary>
        /// <param name="video"></param>
        public void PlayCutScene(VideoPlayer video)
        {
            //pepares the video for playback
            video.Prepare();
            //checks if video is ready for play then plays it
            if (video.isPrepared)
                video.Play();
        }

        /// <summary>
        /// restarts the active scene
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        public void OpenMenu(string menuName)
        {

        }

        public void OpenOptions()
        {

        }

    }
}