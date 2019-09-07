using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartSaves.SaveSystems
{
    public class PersistentDataPathFile<T> : SaveSystem<T> where T : Data<T>
    {
        #region Variables

        private string fileName;
        private string filePath;

        private Config config;

        #endregion

        #region Constructor

        public PersistentDataPathFile(Data<T> _data, Config _config) : base(_data, _config)
        {
            fileName = _data.name;
            filePath = Application.persistentDataPath + "/" + fileName + ".save";
            config = _config;
        }

        #endregion

        #region Methods

        public override void Save()
        {
            // Convert data object to json
            string dataJson = JsonUtility.ToJson(data, true);

            // Shuffle
            switch (config.PersistentDataPathFileShuffle)
            {
                case Config.PersistentDataPathFileShuffleTypes.Random:
                    int keyRandom = UnityEngine.Random.Range(0, 1000);
                    dataJson = Utils.Shuffle(dataJson, keyRandom);
                    dataJson += keyRandom.ToString("000");
                    break;
                case Config.PersistentDataPathFileShuffleTypes.DeviceId:
                    int keyDeviceId = SystemInfo.deviceUniqueIdentifier.GetHashCode();
                    dataJson = Utils.Shuffle(dataJson, keyDeviceId);
                    break;
            }

            // Add checksum
            if(config.PersistentDataPathFileChecksum)
            {
                string md5Sum = Utils.Md5Sum(dataJson);
                dataJson += md5Sum;
            }

            try
            {
                if(!config.PersistentDataPathFileBinary)
                {
                    // Write on file normally
                    File.WriteAllText(filePath, dataJson);
                }
                else
                {
                    // Write on file in binary
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    FileStream fileStream = File.Create(filePath);
                    binaryFormatter.Serialize(fileStream, dataJson);
                    fileStream.Close();
                }
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
                    string dataJson = "";
                    if (!config.PersistentDataPathFileBinary)
                    {
                        // Read on file normally
                        dataJson = File.ReadAllText(filePath);
                    }
                    else
                    {
                        // Read on file in binary
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        FileStream fileStream = File.Open(filePath, FileMode.Open);
                        dataJson = (string)binaryFormatter.Deserialize(fileStream);
                        fileStream.Close();
                    }

                    // Check checksum
                    if(config.PersistentDataPathFileChecksum)
                    {
                        string md5Sum = dataJson.Substring(dataJson.Length - 32, 32);
                        dataJson = dataJson.Substring(0, dataJson.Length - 32);

                        if (Utils.Md5Sum(dataJson) != md5Sum)
                        {
                            Debug.Log("Save as been changed !");
                            return;
                        }
                    }

                    // Unshuffle
                    switch (config.PersistentDataPathFileShuffle)
                    {
                        case Config.PersistentDataPathFileShuffleTypes.Random:
                            int keyRandom = int.Parse(dataJson.Substring(dataJson.Length - 3, 3));
                            dataJson = dataJson.Substring(0, dataJson.Length - 3);
                            dataJson = Utils.Unshuffle(dataJson, keyRandom);
                            break;
                        case Config.PersistentDataPathFileShuffleTypes.DeviceId:
                            int keyDeviceId = SystemInfo.deviceUniqueIdentifier.GetHashCode();
                            dataJson = Utils.Unshuffle(dataJson, keyDeviceId);
                            break;
                    }

                    // Overwrite the data object
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