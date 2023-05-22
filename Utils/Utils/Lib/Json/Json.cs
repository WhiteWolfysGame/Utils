using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Utils.Lib.Json
{
    /// <summary>
    /// Bietet Methoden zum Konvertieren von JSON-Strings und -Objekten sowie 
    /// zum Speichern und Laden von Objekten in und aus JSON-Dateien.
    /// Diese Klasse verwendet die Newtonsoft.Json-Bibliothek.
    /// </summary>
    public class Json
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="Json"/>-Klasse
        /// </summary>
        public Json() { }

        /// <summary>
        /// Gibt eine JSON-String-Repräsentation des angegebenen Objekts zurück.
        /// </summary>
        /// <param name="obj">Das zu serialisierende Objekt.</param>
        /// <returns>Eine JSON-String-Darstellung des angegebenen Objekts.</returns>
        public string GetJsonStringFromObject(object obj)
        {
            return GetJsonStringFromObject(obj, false);
        }

        /// <summary>
        /// Gibt eine JSON-String-Darstellung des angegebenen Objekts zurück, optional formatiert.
        /// </summary>
        /// <param name="obj">Das Objekt, das in einen JSON-String serialisiert werden soll.</param>
        /// <param name="indentedFormat">True, um eine formatierte Ausgabe zu verwenden, andernfalls False.</param>
        /// <returns>Eine JSON-String-Darstellung des angegebenen Objekts.</returns>
        public string GetJsonStringFromObject(object obj, bool indentedFormat)
        {
            if (indentedFormat)
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
            else
            {
                return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
        }

        /// <summary>
        /// Speichert das angegebene Objekt in einer JSON-Datei.
        /// </summary>
        /// <param name="filename">Der Name der Datei, in der das Objekt gespeichert werden soll.</param>
        /// <param name="obj">Das zu speichernde Objekt.</param>
        public void SaveJsonObjectToFile(string filename, object obj)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write(GetJsonStringFromObject(obj));
            sw.Close();
        }

        /// <summary>
        /// Speichert das angegebene Objekt in einer JSON-Datei mit optionaler Formatierung.
        /// </summary>
        /// <param name="filename">Der Name der Datei, in der das Objekt gespeichert werden soll.</param>
        /// <param name="obj">Das zu speichernde Objekt.</param>
        /// <param name="indentedFormat">True, um eine formatierte Ausgabe zu verwenden, andernfalls False.</param>
        public void SaveJsonObjectToFile(string filename, object obj, bool indentedFormat)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write(GetJsonStringFromObject(obj, indentedFormat));
            sw.Close();
        }

        /// <summary>
        /// Liest eine JSON-Datei und gibt das deserialisierte Objekt zurück.
        /// </summary>
        /// <param name="filename">Der Name der zu lensenden Datei.</param>
        /// <returns>Das deserialisierte Objekt</returns>
        public dynamic GetObjectFromFile(string filename)
        {
            string json = ReadJsonFile(filename);
            return JsonConvert.DeserializeObject<dynamic>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }


        /// <summary>
        /// Gibt den Objekt des angegebenen Typs aus einer JSON-Datei zurück.
        /// </summary>
        /// <typeparam name="T">Der Typ des zu deserialisierenden Objekts.</typeparam>
        /// <param name="filename">Der Pfad zur JSON-Datei.</param>
        /// <returns>Das deserialisierte Objekt.</returns>
        public T GetObjectFromFile<T>(string filename)
        {
            string json = ReadJsonFile(filename);
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        /// <summary>
        /// Liest eine JSON-Datei und gibt den Inhalt als String zurück.
        /// </summary>
        /// <param name="jsonfile">Der Pfad zur JSON-Datei</param>
        /// <returns>Der Inhalt der JSON-Datei als String.</returns>
        private string ReadJsonFile(string jsonfile)
        {
            string json = "";
            using (StreamReader sr = new StreamReader(jsonfile))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }
            return json;
        }

        /// <summary>
        /// Gibt ein dynamisches Objekt aus einem JSON-String zurück.
        /// </summary>
        /// <param name="json">Der JSON-String.</param>
        /// <returns>Das deserialisierte dynamische Objekt.</returns>
        public dynamic GetObjectFromJsonString(string json)
        {
            return JsonConvert.DeserializeObject<dynamic>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        /// <summary>
        /// Gibt ein Objekt des angegebenen Typs aus einem JSON-String zurück.
        /// </summary>
        /// <typeparam name="T">Der Typ des zu deserialisierenden Objekts.</typeparam>
        /// <param name="json">Der JSON-String</param>
        /// <returns>Das deserialisierte Objekt</returns>
        public T GetObjectFromJsonString<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}
