using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SmartSaves.SaveSystems
{
    [Serializable]
    public class Config
    {
        #region Enums

        public enum PersistentDataPathFileShuffleTypes
        {
            None,
            Random,
            DeviceId,
        }

        #endregion

        #region Variables

        private Type configType = null;
        public Type ConfigType { get { return configType; } }

        [SerializeField]
        private bool persistentDataPathFileBinary = false;
        public bool PersistentDataPathFileBinary { get { return persistentDataPathFileBinary; } }

        [SerializeField]
        private bool persistentDataPathFileChecksum = false;
        public bool PersistentDataPathFileChecksum { get { return persistentDataPathFileChecksum; } }

        [SerializeField]
        private PersistentDataPathFileShuffleTypes persistentDataPathFileShuffle = PersistentDataPathFileShuffleTypes.None;
        public PersistentDataPathFileShuffleTypes PersistentDataPathFileShuffle { get { return persistentDataPathFileShuffle; } }

        #endregion

        #region Constructor

        public static Config ForPersistentDataPathFile<T>(bool binary = false, bool checksum = false, PersistentDataPathFileShuffleTypes shuffle = PersistentDataPathFileShuffleTypes.None) where T : Data<T>
        {
            Config b_saveSystemConfig = new Config();
            b_saveSystemConfig.configType = typeof(PersistentDataPathFile<T>);
            b_saveSystemConfig.persistentDataPathFileBinary = binary;
            b_saveSystemConfig.persistentDataPathFileChecksum = checksum;
            b_saveSystemConfig.persistentDataPathFileShuffle = shuffle;
            return b_saveSystemConfig;
        }

#if UNITY_EDITOR
        public static void EditorPersistentDataPathFileInspector(ref Settings.Setting _setting, int _configIndex)
        {
            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            bool b_binary = EditorGUILayout.Toggle("Binary", _setting.EditorConfigs[_configIndex].SaveSystemConfig.persistentDataPathFileBinary);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_setting, b_binary ? "Enable" : "Disable" + " binary on save system");
                _setting.EditorConfigs[_configIndex].SaveSystemConfig.persistentDataPathFileBinary = b_binary;
                EditorUtility.SetDirty(_setting);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            bool b_checksum = EditorGUILayout.Toggle("Checksum", _setting.EditorConfigs[_configIndex].SaveSystemConfig.persistentDataPathFileChecksum);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_setting, b_binary ? "Enable" : "Disable" + " checksum on save system");
                _setting.EditorConfigs[_configIndex].SaveSystemConfig.persistentDataPathFileChecksum = b_checksum;
                EditorUtility.SetDirty(_setting);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            PersistentDataPathFileShuffleTypes b_shuffleType = (PersistentDataPathFileShuffleTypes)(EditorGUILayout.EnumPopup("Shuffle", _setting.EditorConfigs[_configIndex].SaveSystemConfig.persistentDataPathFileShuffle));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_setting, "Change shuffle type on save system");
                _setting.EditorConfigs[_configIndex].SaveSystemConfig.persistentDataPathFileShuffle = b_shuffleType;
                EditorUtility.SetDirty(_setting);
            }
            GUILayout.EndHorizontal();
        }
#endif

        #endregion
    }
}