using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartSaves.SaveSystems
{
    public class PersistentDataPathFileBinaryChecksum<T> : SaveSystem<T> where T : Data<T>
    {
        #region Variables

        private string fileName;
        private string filePath;

        #endregion

        #region Constructor

        public PersistentDataPathFileBinaryChecksum(Data<T> _data) : base(_data)
        {
            fileName = _data.name;
            filePath = Application.persistentDataPath + "/" + fileName + ".save";
        }

        #endregion

        #region Methods

        public override void Save()
        {
            string dataJson = JsonUtility.ToJson(data);
            string md5Sum = Utils.Md5Sum(dataJson);
            string fileData = dataJson + md5Sum;
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Create(filePath);
                binaryFormatter.Serialize(fileStream, fileData);
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
                    string fileData = (string)binaryFormatter.Deserialize(fileStream);
                    fileStream.Close();
                    string dataJson = fileData.Substring(0, fileData.Length - 32);
                    string md5Sum = fileData.Substring(fileData.Length - 32, 32);

                    if(Utils.Md5Sum(dataJson) != md5Sum)
                    {
                        Debug.Log("Save as been changed !");
                        return;
                    }

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