using System;
using System.IO;
using System.Xml.Serialization;
using GenericManagers;

public class S_GameManager : Singleton<S_GameManager>
{
    /// <summary>
    ///     
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool Save<T>(string fileName, T obj)
    {
        var xs = new XmlSerializer(typeof(T));
        using (TextWriter sw = new StreamWriter(fileName))
        {
            xs.Serialize(sw, obj);
        }
        return File.Exists(fileName);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T Load<T>(string fileName)
    {
        if (File.Exists(fileName))
        {
            var xs = new XmlSerializer(typeof(T));

            object rslt;
            using (var sr = new StreamReader(fileName))
            {
                rslt = (T)xs.Deserialize(sr);
            }
            return (T)rslt;
        }
        return default;
    }
}
