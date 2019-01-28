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
        Renderer screenRenderer;
        bool fadeDone = false;

        public void Awake()
        {
            // Will attach a VideoPlayer to the main camera.
            camera = Camera.main;
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


        /// <summary>
        /// creates screen used for effects and places in correct position
        /// </summary>
        public void SetupEffectScreen()
        {
            screen = GameObject.CreatePrimitive(PrimitiveType.Plane);
            screen.transform.SetParent(camera.transform);
            screen.transform.localPosition = new Vector3(0, 0, camera.nearClipPlane + .00001f);
            screen.transform.Rotate(Vector3.right, -90, Space.Self);
            screenRenderer = screen.GetComponent<Renderer>();
            screenRenderer.material.color = new Color(0, 0, 0, 0);
            ChangeRenderMode();
        }

        private void ChangeRenderMode()
        {
            screenRenderer.material.SetFloat("_Mode", 2);
            screenRenderer.material.SetFloat("_Metallic", 1);
            screenRenderer.material.SetFloat("_Glossiness", 0);
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
                Debug.LogWarning("file does not exist");
            _videoPlayer.url = videoFilePath;
            //plays the video clip
            _videoPlayer.Play();
        }

        public void SetScreenColor(float r, float g, float b, float a)
        {
            if (!screen)
                SetupEffectScreen();
            screenRenderer.material.color = new Color(r, g, b, a);

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
        private Image CreateBlindfold()
        {
            var blindfold = new GameObject();
            blindfold.transform.SetParent(FindObjectOfType<Canvas>().transform);
            blindfold.AddComponent<Image>().color = Color.black;
            var rect = blindfold.GetComponent<RectTransform>();
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.anchorMin = new Vector2(0, 0f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            return blindfold.GetComponent<Image>();
        }

        /// <summary>
        /// Use this to fade screen over the game
        /// </summary>
        /// <param name="aValue">Value the screen will fade to.</param>
        /// <param name="aTime">Duration of fade.</param>
        /// <returns></returns>
        public IEnumerator FadeScene(float aValue, float aTime)
        {
            var blindfold = CreateBlindfold();

            if (blindfold != null)
            {
                Color c = blindfold.GetComponent<Image>().color;
                c.a = 1f;
                blindfold.GetComponent<Image>().color = c;

                while (blindfold.GetComponent<Image>().color.a > (aValue + 0.01f))
                {
                    c.a -= (Time.deltaTime / aTime);
                    blindfold.GetComponent<Image>().color = c;
                    yield return null;
                }

                c.a = aValue;
                blindfold.GetComponent<Image>().color = c;
            }
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
                SetupEffectScreen();
            bool minHit = true, maxHit = false;
            float alpha = screenRenderer.material.color.a;
            var t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                Debug.Log(t);
                if (maxHit)
                {
                    for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / time)
                    {
                        var p = i * flashSpeed;
                        Color newColor = new Color(screenRenderer.material.color.r, screenRenderer.material.color.g, screenRenderer.material.color.b, Mathf.Lerp(screenRenderer.material.color.a, minFade, p));
                        screenRenderer.material.color = newColor;
                        if (screenRenderer.material.color.a <= minFade)
                        {
                            minHit = true;
                            maxHit = false;
                            break;
                        }
                        yield return null;
                    }
                    yield return null;
                }
                else if (minHit)
                {
                    for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / time)
                    {
                        var p = i * flashSpeed;
                        Color newColor = new Color(screenRenderer.material.color.r, screenRenderer.material.color.g, screenRenderer.material.color.b, Mathf.Lerp(screenRenderer.material.color.a, maxFade, p));
                        screenRenderer.material.color = newColor;
                        if (screenRenderer.material.color.a >= maxFade)
                        {
                            minHit = false;
                            maxHit = true;
                            break;
                        }
                        yield return null;
                    }
                    yield return null;
                }
                yield return null;
            }
            DestroyImmediate(screen);
            yield return null;
        }

    }
}