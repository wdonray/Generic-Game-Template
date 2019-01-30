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
        Camera camera;
        GameObject screen;
        UnityEvent fade;

        Image screenImage;

        float aTime = 5;
        bool fadeDone = false;
        public void Awake()
        {
            // Will attach a VideoPlayer to the main camera.
            camera = Camera.main;
        }
        //TODO THIS BAD WILL FIX
        public void Update()
        {
            if (fadeDone)
                StartCoroutine(Test());
        }

        public void SetupVideoPlayer()
        {
            // VideoPlayer automatically targets the camera backplane when it is added
            // to a camera object, no need to change videoPlayer.targetCamera.
            _videoPlayer = camera.gameObject.AddComponent<VideoPlayer>();

            // Play on awake defaults to true. Set it to false to avoid the url set
            // below to auto-start playback since we're in Start().
            _videoPlayer.playOnAwake = false;

            // By default, VideoPlayers added to a camera will use the far plane.
            // Let's target the near plane instead.
            _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            _videoPlayer.SetDirectAudioVolume(0, .2f);
        }


        private void ChangeRenderMode(GameObject go)
        {
            var g = go.GetComponent<Renderer>().material;
            g.SetFloat("_Mode", 2);
            g.SetFloat("_Metallic", 1);
            g.SetFloat("_Glossiness", 0);
            g.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            g.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            g.SetInt("_ZWrite", 0);
            g.DisableKeyword("_ALPHATEST_ON");
            g.EnableKeyword("_ALPHABLEND_ON");
            g.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            g.renderQueue = 3000;
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
            if (speed < 0)
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
                UnityEngine.Debug.LogWarning("file does not exist");
            _videoPlayer.url = videoFilePath;
            //plays the video clip
            _videoPlayer.Play();
        }

        public void SetScreenColor(float r, float g, float b, float a)
        {
            if (!screen)
                CreateScreen();
            screenImage.color = new Color(r, g, b, a);

        }

        /// <summary>
        /// Restarts the active scene.
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        /// <summary>
        ///     
        /// </summary>
        /// <returns></returns>
        private void CreateScreen()
        {
            screen = new GameObject();
            screen.transform.SetParent(FindObjectOfType<Canvas>().transform);
            screenImage = screen.AddComponent<Image>();
            screenImage.color = Color.clear;
            var rect = screen.GetComponent<RectTransform>();
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.anchorMin = new Vector2(0, 0f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }
        public string s = string.Empty;

        /// <summary>
        /// Use this to fade screen over the game
        /// </summary>
        /// <param name="aValue">Value the screen will fade to.</param>
        /// <param name="aTime">Duration of fade.</param>
        /// <returns></returns>
        public IEnumerator FadeScene(string sceneToFadeTo)
        {
            if (!screen)
                CreateScreen();
            DontDestroyOnLoad(screen.transform.parent.gameObject);

            float alpha = screen.GetComponent<Image>().color.a;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(screenImage.color.r, screenImage.color.b, screenImage.color.g, Mathf.Lerp(alpha, 1, t));
                screenImage.color = newColor;
                yield return null;
            }
            s = SceneManager.GetSceneByName(sceneToFadeTo).ToString();
            var otherS = SceneManager.GetSceneByName(s);

            SceneManager.LoadScene(sceneToFadeTo);
            fadeDone = true;
            yield return new WaitUntil(() => otherS.isLoaded);
            print("is Loaded");
           

            //Pass in a string value for the scene to fade to
            //It will then fade in an image from 0 to the passed in value
            //It will then store that image using DontDestroyOnLoad(screen)
            //Once the scene has loaded you can then fade out the image from passed in value to 0

            //Things to note you can use (yield return WaitUntil(*Function*) to wait until the scene and fully loaded to then fade out the image)
            //Have a bool to set true while fading of the image is in effect so stuff like player movement and such can read from this and wait until its false
        }

        private IEnumerator Test()
        {
            float alpha = screen.GetComponent<Image>().color.a;
            for (float x = 0.0f; x < 1.0f; x += Time.deltaTime / aTime)
            {
                Color newColor = new Color(screenImage.color.r, screenImage.color.b, screenImage.color.g, Mathf.Lerp(alpha, 0, x));
                screenImage.color = newColor;
                yield return null;
            }
            fadeDone = false;
        }


        public IEnumerator FadeObject()
        {
            yield return null;
        }
        /// <summary>
        /// Gives screen a flashing effect over game.
        /// </summary>
        /// <param name="minFade">Minimun value screen should hit when fading in.</param>
        /// <param name="maxFade">Maximum value screen should hit when fadine out.</param>
        /// <param name="time">How long the flashing should take place.</param>
        /// <param name="flashSpeed">How fast the speed of the flash will be</param>
        /// <returns></returns>        
        public IEnumerator FlashScreen(float minFade, float maxFade, float time, float flashSpeed)
        {
            if (!screen)
                CreateScreen();
            bool minHit = true, maxHit = false;
            float alpha = screenImage.color.a;
            float counter = time;
            while (counter > 0)
            {
                //Logic
                if (maxHit)
                {
                    for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / time)
                    {
                        var p = i * flashSpeed;
                        Color newColor = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, Mathf.Lerp(screenImage.color.a, minFade, p));
                        screenImage.color = newColor;
                        if (screenImage.color.a <= minFade)
                        {
                            minHit = true;
                            maxHit = false;
                            break;
                        }
                    }
                }
                else if (minHit)
                {
                    for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / time)
                    {
                        var p = i * flashSpeed;
                        Color newColor = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, Mathf.Lerp(screenImage.color.a, maxFade, p));
                        screenImage.color = newColor;
                        if (screenImage.color.a >= maxFade)
                        {
                            minHit = false;
                            maxHit = true;
                            break;
                        }
                    }
                }
                yield return new WaitForSeconds(1f);
                counter--;
            }
            DestroyImmediate(screen);
            print("Finished");
        }
    }
}