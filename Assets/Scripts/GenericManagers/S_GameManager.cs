using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace GenericManagers
{
    /// <summary>
    ///     Created by: Donray Williams 2018
    /// </summary>
    public class S_GameManager : Singleton<S_GameManager>
    {
        public UnityEvent OnQuitEvent, OnSaveEvent, OnLoadEvent;

        void OnEnable()
        {
            OnQuitEvent = new UnityEvent(); OnSaveEvent = new UnityEvent(); OnLoadEvent = new UnityEvent();
        }

        /// <summary>
        ///     Attempt to save generic file using XML if unable to catch it and display error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="pathSpec"></param>
        public void Serialize<T>(T obj, string pathSpec)
        {
            try
            {
                OnSaveEvent?.Invoke();
                var serializer = new XmlSerializer(typeof(T));
                var textWriter = new StreamWriter(pathSpec);
                serializer.Serialize(textWriter, obj);
                textWriter.Close();
            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to serialize {obj} into file {pathSpec}";
                throw new Exception(errMsg, sX);
            }
        }

        /// <summary>
        ///     Load a saved file and if unable to catch it and display error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathSpec"></param>
        /// <returns></returns>
        public T DeSerialize<T>(string pathSpec) where T : class
        {
            try
            {
                OnLoadEvent?.Invoke();
                var serializer = new XmlSerializer(typeof(T));
                var rdr = new StreamReader(pathSpec);
                return (T)serializer.Deserialize(rdr);

            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to deserialize {typeof(T)} from file {pathSpec}";
                throw new Exception(errMsg, sX);
            }
        }

        /// <summary>
        ///     Exits the application and calls all needed events
        /// </summary>
        public void Quit()
        {
            OnQuitEvent?.Invoke();
            Application.Quit();
        }

        /// <summary>
        ///     Add To to Quit Event
        /// </summary>
        /// <param name="unityEvent"></param>
        /// <param name="call"></param>
        public void AddToEvent(UnityEvent unityEvent, [NotNull] UnityAction call)
        {
            if (call == null) throw new ArgumentNullException(nameof(call));
            unityEvent.AddListener(call);
        }
    }
}