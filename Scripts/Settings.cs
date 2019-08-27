using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SmartSaves
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class SmartSaveSettingsInitializeOnLoad
    {
        static SmartSaveSettingsInitializeOnLoad()
        {
            if (Settings.Instance == null)
                Debug.Log("Impossible to initialize SmartSaves settings");
        }
    }
#endif

    public class Settings : ScriptableObject
    {
        #region Consts

        private const string assetName = "SmartSaveSettings";
        private const string assetPath = "SmartSave/Resources/" + assetName + ".asset";
        private const string scriptName = "Settings";
        private const string scriptPath = "SmartSave/Script/" + scriptName + ".cs";

        #endregion

        #region Variables

        private static Settings instance;
#if UNITY_EDITOR
        public static Settings Instance { get { if (!instance) Instanciate(); return instance; } }
#else
        public static Settings Instance { get { return instance; } }
#endif

        [SerializeField]
        private SaveTypes saveType = SaveTypes.BinaryChecksum;
        public SaveTypes SaveType { get { return saveType; } }

        #endregion

        #region Methods

#if UNITY_EDITOR
        private static void Instanciate()
        {
            // Try load save
            string[] assetGUIDs = AssetDatabase.FindAssets("t:" + typeof(Settings).ToString());
            if (assetGUIDs.Length > 0)
                instance = AssetDatabase.LoadAssetAtPath<Settings>(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]));

            // If no one found, create one
            if (!instance)
            {
                // Search path
                string assetFullPath = "";
                string b_assetFullPath;
                assetGUIDs = AssetDatabase.FindAssets(scriptName + " t:Script");
                foreach (string iGUID in assetGUIDs)
                {
                    b_assetFullPath = AssetDatabase.GUIDToAssetPath(iGUID);
                    if (b_assetFullPath.Contains(scriptPath))
                    {
                        assetFullPath = b_assetFullPath.Replace(scriptPath, assetPath);
                        break;
                    }
                }

                // In case don't find create at root
                if (assetFullPath == "")
                    assetFullPath = "Assets/" + assetPath;

                // Create directory
                string directoryPath = assetFullPath.Replace("/" + assetName + ".asset", "");
                System.IO.Directory.CreateDirectory(directoryPath);

                instance = CreateInstance<Settings>();
                AssetDatabase.CreateAsset(instance, assetFullPath);
                AssetDatabase.SaveAssets();
            }
        }
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            instance = Resources.Load<Settings>(assetName + ".asset");
            if (!instance)
                instance = CreateInstance<Settings>();
        }
#endif

        #endregion
    }
}