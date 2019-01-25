using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;

namespace GenericManagers
{
    /// <summary>
    ///     Created by: Reginald Reed 2018
    /// </summary>
    public class S_LevelManager : Singleton<S_LevelManager>
    {
        private VideoPlayer _videoPlayer;
        GameObject camera;

        GameObject screen;
        Renderer screenRenderer;
        AsyncOperation sceneAsync;
        IEnumerator FadeOut;
        public void Awake()
        {
            // Will attach a VideoPlayer to the main camera.
            camera = GameObject.Find("Main Camera");
        }

        public void SetupVideoPlayer()
        {
            // VideoPlayer automatically targets the camera backplane when it is added
            // to a camera object, no need to change videoPlayer.targetCamera.
            _videoPlayer = camera.AddComponent<VideoPlayer>();

            // Play on awake defaults to true. Set it to false to avoid the url set
            // below to auto-start playback since we're in Start().
            _videoPlayer.playOnAwake = false;

            // By default, VideoPlayers added to a camera will use the far plane.
            // Let's target the near plane instead.
            _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            _videoPlayer.SetDirectAudioVolume(0, .2f);
        }

        private void SetupEffectScreen()
        {
            screen = GameObject.CreatePrimitive(PrimitiveType.Plane);
            screen.transform.SetParent(camera.transform);
            screen.transform.Translate(new Vector3(0, 0, -9), Space.Self);
            screen.transform.Rotate(Vector3.right, -90, Space.Self);
            screenRenderer = screen.GetComponent<Renderer>();
           // screenRenderer.material.color = new Color(0, 0, 0, 0);
            ChangeRenderMode();
        }

        private void ChangeRenderMode()
        {
            screenRenderer.material.SetFloat("_Mode", 2);
            screenRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            screenRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            screenRenderer.material.SetInt("_ZWrite", 0);
            screenRenderer.material.DisableKeyword("_ALPHATEST_ON");
            screenRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            screenRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            screenRenderer.material.renderQueue = 3000;
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
            if (speed >= 1)
                speed = 1;
            else if (speed < 0)
                speed = 0;

            Time.timeScale = speed;
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
        /// play video/cutscene. Takes in the file path to video
        /// </summary>
        /// <param name="video"></param>
        public void PlayCutScene(string videoFilePath)
        {
            SetupVideoPlayer();
            if (videoFilePath == null)
                Debug.LogWarning("file does not exist");
            _videoPlayer.url = videoFilePath;
            //plays the video clip
            _videoPlayer.Play();
        }

        /// <summary>
        ///     restarts the active scene
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        public IEnumerator FadeOutScene()
        {
            SetupEffectScreen();
            screenRenderer.material.color = new Color(0, 0, 0, 0);

            while (true)
            {
                screenRenderer.material.color += new Color(0, 0, 0, .01f);
                if (screenRenderer.material.color.a >= 1)
                {
                    yield return null;
                }
            }
        }


        public IEnumerator FadeInScene()
        {
            //SetupEffectScreen();
            screenRenderer.material.color = new Color(0, 0, 0, 1);
            while (true)
            {
                screenRenderer.material.color -= new Color(0, 0, 0, .01f);

                if (screenRenderer.material.color.a <= 0)
                    yield return null;
            }
        }


        public void FadeToScene()
        {
            SetupEffectScreen();

            StartCoroutine(FadeOutScene());

            StartCoroutine(FadeInScene());
        }





    }
}