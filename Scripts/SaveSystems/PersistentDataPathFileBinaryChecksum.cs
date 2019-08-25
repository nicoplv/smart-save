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

        // TODO Move this method outside the class and use it to "rename" the files
        public string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        public override void Save()
        {
            string dataJson = JsonUtility.ToJson(data);
            string key = Md5Sum(dataJson);
            string fileData = dataJson + key;
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
                    string key = fileData.Substring(fileData.Length - 32, 32);

                    if(Md5Sum(dataJson) != key)
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