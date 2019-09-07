using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SmartSaves.SaveSystems;

#if UNITY_EDITOR
using UnityEditor.Build.Reporting;
using UnityEditor;
#endif

namespace SmartSaves.Settings
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class SmartSaveSettingsInitializeOnLoad
    {
        static SmartSaveSettingsInitializeOnLoad()
        {
            if (Setting.Instance == null)
                Debug.Log("Impossible to initialize SmartSaves settings");
        }
    }

    public class SettingPreprocessBuild : UnityEditor.Build.IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return -1 ; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            // Set the good config in function of the target
            Setting.Instance.EditorConfig = Setting.Instance.EditorGetBuildTargetConfig(report.summary.platform);
            EditorUtility.SetDirty(Setting.Instance);
            AssetDatabase.SaveAssets();
        }
    }

#endif

    public class Setting : ScriptableObject
    {
        #region Consts

        private const string assetName = "SmartSaveSettings";
        private const string assetPath = "SmartSave/Resources/" + assetName + ".asset";
        private const string scriptName = "Settings";
        private const string scriptPath = "SmartSave/Script/" + scriptName + ".cs";

        #endregion

        #region Setting Config Class

        [Serializable]
        public class SettingConfig
        {
            #region Variables

            public string Name = "Unnamed";

#if UNITY_EDITOR
            public List<UnityEditor.BuildTarget> EditorBuildTargets = new List<BuildTarget>();
#endif

            // TODO Use that to select the active build target
            //if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)

            public SaveSystems.Types SaveSystemType = SaveSystems.Types.PersistentDataPathFile;

            public Config SaveSystemConfig = new Config();

            #endregion
        }

        #endregion

        #region Variables

        private static Setting instance;
#if UNITY_EDITOR
        public static Setting Instance { get { if (!instance) Instanciate(); return instance; } }
#else
        public static Setting Instance { get { return instance; } }
#endif

        [SerializeField]
        private SettingConfig config;
        public SettingConfig Config { get { return config; } }

#if UNITY_EDITOR
        public SettingConfig EditorConfig { set { config = value; } }

        [SerializeField]
        private List<SettingConfig> editorConfigs = new List<SettingConfig>();
        public List<SettingConfig> EditorConfigs { get { return editorConfigs; } }

        public int callbackOrder => throw new NotImplementedException();
#endif

#endregion

#region Methods

#if UNITY_EDITOR
        public SettingConfig EditorGetBuildTargetConfig(BuildTarget _buildTarget)
        {
            foreach(SettingConfig iSettingConfig in editorConfigs)
            {
                if (iSettingConfig.EditorBuildTargets.Contains(_buildTarget))
                    return iSettingConfig;
            }

            // If find no config set the default config
            return new SettingConfig();
        }
#endif

#if UNITY_EDITOR
        private static void Instanciate()
        {
            // Try load save
            string[] assetGUIDs = AssetDatabase.FindAssets("t:" + typeof(Setting).ToString());
            if (assetGUIDs.Length > 0)
                instance = AssetDatabase.LoadAssetAtPath<Setting>(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]));

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

                instance = CreateInstance<Setting>();
                AssetDatabase.CreateAsset(instance, assetFullPath);
                AssetDatabase.SaveAssets();
            }
        }
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            instance = Resources.Load<Setting>(assetName);
            if (!instance)
                instance = CreateInstance<Setting>();
        }
#endif

#endregion
    }
}