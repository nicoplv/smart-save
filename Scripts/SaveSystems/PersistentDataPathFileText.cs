using UnityEngine;
using System.IO;

namespace SmartSaves.SaveSystems
{
	public class PersistentDataPathFileText<T> : SaveSystem<T> where T : Data<T>
    {
        #region Variables

        private string fileName;
        private string filePath;

        #endregion

        #region Constructor

        public PersistentDataPathFileText(Data<T> _data) : base(_data)
        {
            fileName = _data.name;
            filePath = Application.persistentDataPath + "/" + fileName + ".save";
        }
        
        #endregion

        #region Methods

        public override void Save()
        {
            string dataJson = JsonUtility.ToJson(data, true);
            try
            {
                File.WriteAllText(filePath, dataJson);
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
                    string dataJson = File.ReadAllText(filePath);
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