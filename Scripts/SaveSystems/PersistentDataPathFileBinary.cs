using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartSaves.SaveSystems
{
    public class PersistentDataPathFileBinary<T> : SaveSystem<T> where T : Data<T>
    {
        #region Variables

        private string fileName;
        private string filePath;

        #endregion

        #region Constructor

        public PersistentDataPathFileBinary(Data<T> _data) : base(_data)
        {
            fileName = _data.name;
            filePath = Application.persistentDataPath + "/" + fileName + ".save";
        }

        #endregion

        #region Methods

        public override void Save()
        {
            string dataJson = JsonUtility.ToJson(data);
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Create(filePath);
                binaryFormatter.Serialize(fileStream, dataJson);
                fileStream.Close();
            }
            catch
            {
                Debug.LogError("Error writing the file \"" + fileName + "\"");
            }
        }

        public override void Load()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    FileStream fileStream = File.Open(filePath, FileMode.Open);
                    string dataJson = (string)binaryFormatter.Deserialize(fileStream);
                    fileStream.Close();
                    JsonUtility.FromJsonOverwrite(dataJson, data);
                }
                catch
                {
                    Debug.LogError("Error reading the file \"" + fileName + "\"");
                }
            }
        }

        public override void Unload()
        {
        }

        public override void Delete()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    Debug.LogError("Error deletion the file \"" + fileName + "\"");
                }
            }
        }

        #endregion
    }
}