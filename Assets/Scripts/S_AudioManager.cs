using System.Collections;
using UnityEngine;

namespace GenericManagers
{
    public class S_AudioManager : Singleton<S_AudioManager>
    {
        private float _lowPitchRange = .95f;
        private float _highPitchRange = 1.05f;
        private bool _userPaused = false;

        /// <summary>
        ///     Play any clip by passing in a string
        ///     Must match the actual file name
        ///     Must be in this folder structure Resources/Audio/ClipName
        /// </summary>
        /// <param name="clipName"></param>
        public void PlayClip(string clipName)
        {
            StartCoroutine(PlayClipEnumerator(clipName, false, false));
        }

        /// <summary>
        ///     Search for the clip by the given string and fade out to the given falue by the time to fade
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="fadeValue"></param>
        /// <param name="timeToFade"></param>
        public void FadeOutClip(string clipName, float fadeValue, float timeToFade)
        {
            StartCoroutine(FadeOutClipEnumerator(clipName, fadeValue, timeToFade));
        }

        /// <summary>
        ///     Search for the clip by the given string and fade in to the given falue by the time to fade
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="fadeValue"></param>
        /// <param name="timeToFade"></param>
        public void FadeInClip(string clipName, float fadeValue, float timeToFade)
        {
            StartCoroutine(FadeInClipEnumerator(clipName, fadeValue, timeToFade));
        }

        /// <summary>
        ///     Play any clip by passing in a string, much like the PlayClip however this will randomize the pitch
        /// </summary>
        /// <param name="clipName"></param>
        public void PlayClipRandomPitch(string clipName)
        {
            StartCoroutine(PlayClipEnumerator(clipName, true, false));
        }

        /// <summary>
        ///     Play any clip by passing in a string, but in reverse
        /// </summary>
        /// <param name="clipName"></param>
        public void PlayAudioInReverse(string clipName)
        {
            StartCoroutine(PlayClipEnumerator(clipName, false, true));
        }

        /// <summary>
        ///     Searches for the clip by the string and destroys it
        /// </summary>
        /// <param name="clipName"></param>
        public void StopPlayingClip(string clipName)
        {
            AudioSource currentAudioSource = GetAudioSource(clipName);

            if (currentAudioSource != null)
            {
                Destroy(currentAudioSource);
            }
            else
            {
                PrintNotFound(clipName);
            }
        }

        /// <summary>
        ///       Searches for the clip by the string and pauses it
        /// </summary>
        /// <param name="clipName"></param>
        public void PauseClip(string clipName)
        {
            var currentAudioSource = GetAudioSource(clipName);

            if (currentAudioSource != null)
            {
                _userPaused = true;
                currentAudioSource.Pause();
            }
            else
            {
                PrintNotFound(clipName);
            }
        }

        /// <summary>
        ///       Searches for the clip by the string and pauses it
        /// </summary>
        /// <param name="clipName"></param>
        public void ResumeClip(string clipName)
        {
            var currentAudioSource = GetAudioSource(clipName);

            if (currentAudioSource != null)
            {
                _userPaused = false;
                currentAudioSource.Play();
            }
            else
            {
                PrintNotFound(clipName);
            }
        }

        /// <summary>
        ///     Finds the clips current audio source
        /// </summary>
        /// <param name="clipName"></param>
        /// <returns></returns>
        public AudioSource GetAudioSource(string clipName)
        {
            var clip = Resources.Load<AudioClip>("Audio/" + clipName);
            AudioSource currentAudioSource = null;
            foreach (var audioSource in GetComponents<AudioSource>())
            {
                if (audioSource.isPlaying)
                {
                    if (audioSource.clip == clip)
                    {
                        currentAudioSource = audioSource;
                    }
                }
            }

            return currentAudioSource;
        }

        /// <summary>
        ///     Set the Low Pitch Value 
        /// </summary>
        /// <param name="value"></param>
        public void SetLowPitch(float value)
        {
            _lowPitchRange = value;
        }

        /// <summary>
        ///     Set the High Pitch Value
        /// </summary>
        /// <param name="value"></param>
        public void SetHighPitch(float value)
        {
            _highPitchRange = value;
        }

        /// <summary>
        ///     Logic for playing a audio clip
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="randomPitchOn"></param>
        /// <returns></returns>
        private IEnumerator PlayClipEnumerator(string clipName, bool randomPitchOn, bool reverse)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            var clip = Resources.Load<AudioClip>("Audio/" + clipName);

            if (clip != null)
            {
                if (audioSource.isPlaying == false)
                {
                    if (randomPitchOn)
                    {
                        var randomPitch = Random.Range(_lowPitchRange, _highPitchRange);
                        audioSource.pitch = randomPitch;
                    }

                    audioSource.clip = clip;

                    if (reverse)
                    {
                        audioSource.timeSamples = audioSource.clip.samples - 1;
                        audioSource.pitch = -1;
                    }
                    audioSource.Play();
                }

                if (_userPaused == false)
                {
                    yield return new WaitUntil(() => CheckIfPlaying(audioSource) == false);
                    Destroy(audioSource);
                }
            }
            else
            {
                PrintNotFound(clipName);
                yield return null;
            }
        }

        /// <summary>
        ///     Logic for fading out an audioclip
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="fadeValue"></param>
        /// <param name="timeToFade"></param>
        /// <returns></returns>
        private IEnumerator FadeOutClipEnumerator(string clipName, float fadeValue, float timeToFade)
        {
            AudioSource currentAudioSource = GetAudioSource(clipName);

            if (currentAudioSource != null)
            {
                float startVolume = currentAudioSource.volume;
                while (currentAudioSource.volume > fadeValue)
                {
                    currentAudioSource.volume -= startVolume * Time.deltaTime / timeToFade;
                    yield return new WaitForEndOfFrame();
                }

                currentAudioSource.volume = fadeValue;

                if (fadeValue <= 0)
                {
                    Destroy(currentAudioSource);
                }
            }
            else
            {
                PrintNotFound(clipName);
                yield return null;
            }
        }

        /// <summary>
        ///     Logic for fading in audioclip
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="fadeValue"></param>
        /// <param name="timeToFade"></param>
        /// <returns></returns>
        private IEnumerator FadeInClipEnumerator(string clipName, float fadeValue, float timeToFade)
        {
            if (fadeValue > 1)
            {
                fadeValue = 1;
            }

            var audioSource = gameObject.AddComponent<AudioSource>();
            var clip = Resources.Load<AudioClip>("Audio/" + clipName);

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.volume = 0f;
                audioSource.Play();

                float startVolume = 0.2f;
                while (audioSource.volume < fadeValue)
                {
                    audioSource.volume += startVolume * Time.deltaTime / timeToFade;
                    yield return new WaitForEndOfFrame();
                }

                audioSource.volume = fadeValue;
                yield return new WaitUntil(() => CheckIfPlaying(audioSource) == false);
                Destroy(audioSource);
            }
            else
            {
                PrintNotFound(clipName);
                yield return null;
            }
        }

        /// <summary>
        ///     Check if audiosource is playing
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        private bool CheckIfPlaying(AudioSource audioSource)
        {
            if (audioSource == null)
            {
                return false;
            }

            if (_userPaused)
            {
                return true;
            }

            return audioSource.isPlaying;
        }

        /// <summary>
        ///     If clip not found print
        /// </summary>
        /// <param name="clipName"></param>
        private void PrintNotFound(string clipName)
        {
            Debug.Log("<color=yellow>[" + typeof(S_AudioManager) + "] " + clipName +
                      " clip was not found </color>");
        }
    }
}