using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using SmartSaves.SaveSystems;

namespace SmartSaves.Settings
{
    [CustomEditor(typeof(Setting))]
    public class SettingEditor : Editor
    {
        #region Static Variables

        private class Content
        {
            public static readonly GUIContent kAddConfig = EditorGUIUtility.TrTextContent("Add Config", "Add new save config");
            public static readonly GUIContent kDeleteConfig = EditorGUIUtility.TrTextContent("Delete Config", "Delete save config");
            public static readonly GUIContent kAddBuildTarget = EditorGUIUtility.TrTextContent("Add Target", "Add build target to the save config");
            public static readonly GUIContent kIconTrash = EditorGUIUtility.TrIconContent("TreeEditor.Trash", "Delete save config");
            public static readonly GUIContent kIconAdd = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add new save config");
        }

        private static class Styles
        {
            public static readonly GUIStyle kToggle = "OL Toggle";
            public static readonly GUIStyle kDefaultToggle = "OL ToggleWhite";

            public static readonly GUIStyle kListEvenBg = "ObjectPickerResultsOdd";
            public static readonly GUIStyle kListOddBg = "ObjectPickerResultsEven";
            public static readonly GUIStyle kDefaultDropdown = "QualitySettingsDefault";

            public const int kMinToggleWidth = 15;
            public const int kMaxToggleWidth = 20;
            public const int kHeaderRowHeight = 20;
            public const int kLabelWidth = 80;
        }

        #endregion

        #region Variables

        //private Settings.SettingConfig selectedConfig = null;
        private static int selectedConfigIndex = 0;

        private static List<BuildTarget> buildTargets;
        private static string[] buildTargetNames;
        private static int buildTargetMask = 0;

        #endregion

        #region Methods

        public override void OnInspectorGUI()
        {
            Setting settingTarget = (Setting)target;

            // Populate build target list if necessary
            if(buildTargets == null)
            {
                buildTargets = ((BuildTarget[])Enum.GetValues(typeof(BuildTarget))).ToList();
                buildTargets.RemoveAll(target => IsObsolete(target));
                buildTargets.Remove(BuildTarget.NoTarget);
                List<string> b_buildTargetNames = new List<string>();
                foreach (BuildTarget iBuildTarget in buildTargets)
                    b_buildTargetNames.Add(iBuildTarget.ToString());
                buildTargetNames = b_buildTargetNames.ToArray();
            }

            Event eventCurrent = Event.current;

            GUILayout.BeginVertical();

            DrawSpace(10);

            // Toolbar
            GUILayout.BeginHorizontal();
            for (int i = 0; i < settingTarget.EditorConfigs.Count; ++i)
            {
                // Set style
                GUIStyle buttonStyle = EditorStyles.miniButtonMid;
                if (i == 0)
                    buttonStyle = EditorStyles.miniButtonLeft;

                // Set variables
                Setting.SettingConfig iSettingConfig = settingTarget.EditorConfigs[i];
                bool selected = i == selectedConfigIndex;
                string displayLabel = iSettingConfig.Name;

                Rect rectButton = GUILayoutUtility.GetRect(GUIContent.none, buttonStyle);

                switch (eventCurrent.type)
                {
                    case EventType.MouseDown:
                        if (rectButton.Contains(eventCurrent.mousePosition))
                        {
                            selectedConfigIndex = i;
                            GUIUtility.keyboardControl = 0;
                            GUIUtility.hotControl = iSettingConfig.GetHashCode();
                            eventCurrent.Use();
                        }
                        break;
                    case EventType.MouseUp:
                        if (GUIUtility.hotControl == iSettingConfig.GetHashCode())
                        {
                            GUIUtility.hotControl = 0;
                            eventCurrent.Use();
                        }
                        break;
                    case EventType.Repaint:
                        buttonStyle.Draw(rectButton, displayLabel, false, false, selected, false);
                        break;
                }
            }

            // Add button
            if (GUI.Button(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniButtonRight), Content.kIconAdd, (settingTarget.EditorConfigs.Count > 0) ? EditorStyles.miniButtonRight : EditorStyles.miniButton))
            {
                Undo.RecordObject(settingTarget, "Add new config");
                Setting.SettingConfig b_config = new Setting.SettingConfig();
                settingTarget.EditorConfigs.Add(b_config);
                selectedConfigIndex = settingTarget.EditorConfigs.Count - 1;
                EditorUtility.SetDirty(settingTarget);
            }
            GUILayout.EndHorizontal();

            DrawSpace(10);

            // Display config settings
            GUILayout.BeginVertical();
            if (selectedConfigIndex >= 0 && selectedConfigIndex < settingTarget.EditorConfigs.Count)
            {
                // Name
                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                string b_name = EditorGUI.TextField(GUILayoutUtility.GetRect(GUIContent.none, Styles.kDefaultToggle), "Name", settingTarget.EditorConfigs[selectedConfigIndex].Name);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(settingTarget, "Rename config");
                    settingTarget.EditorConfigs[selectedConfigIndex].Name = b_name;
                    EditorUtility.SetDirty(settingTarget);
                }
                GUILayout.EndHorizontal();

                DrawSpace(5);

                // Display a list of the build target
                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                buildTargetMask = ListToMask(buildTargets, settingTarget.EditorConfigs[selectedConfigIndex].EditorBuildTargets);
                buildTargetMask = EditorGUI.MaskField(GUILayoutUtility.GetRect(GUIContent.none, Styles.kDefaultToggle), "Used for platform", buildTargetMask, buildTargetNames);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(settingTarget, "Used platform changed");
                    settingTarget.EditorConfigs[selectedConfigIndex].EditorBuildTargets = MaskToList(buildTargets, buildTargetMask);
                    EditorUtility.SetDirty(settingTarget);
                }
                GUILayout.EndHorizontal();

                //string resumeBuildTarget = "";
                //foreach (BuildTarget iBuildTarget in settingTarget.EditorConfigs[selectedConfigIndex].EditorBuildTargets)
                //    resumeBuildTarget += ", " + iBuildTarget.ToString();
                //resumeBuildTarget = resumeBuildTarget.Substring(2, resumeBuildTarget.Length - 2);
                //EditorGUI.LabelField(GUILayoutUtility.GetRect(GUIContent.none, Styles.kDefaultToggle), resumeBuildTarget);

                DrawSpace(5);

                // Delete button
                GUILayout.BeginHorizontal();
                if (GUI.Button(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniButtonRight), Content.kDeleteConfig, EditorStyles.miniButton))
                {
                    if (EditorUtility.DisplayDialog(
                        "Delete " + settingTarget.EditorConfigs[selectedConfigIndex].Name + " Config?",
                        "Are you sure you want to delete " + settingTarget.EditorConfigs[selectedConfigIndex].Name + " config?", "Yes", "No"))
                    {
                        Undo.RecordObject(settingTarget, "Delete " + settingTarget.EditorConfigs[selectedConfigIndex].Name + " Config");
                        settingTarget.EditorConfigs.Remove(settingTarget.EditorConfigs[selectedConfigIndex]);
                        selectedConfigIndex--;
                        if (selectedConfigIndex < 0)
                            selectedConfigIndex = 0;
                        EditorUtility.SetDirty(settingTarget);
                    }
                }
                GUILayout.EndHorizontal();

                DrawLine(1, 10, 10);

                // Save sytem selection
                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                SaveSystems.Types b_saveSystemType = (SaveSystems.Types)EditorGUILayout.EnumPopup("Save System", settingTarget.EditorConfigs[selectedConfigIndex].SaveSystemType);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(settingTarget, "Change save system");
                    settingTarget.EditorConfigs[selectedConfigIndex].SaveSystemType = b_saveSystemType;
                    EditorUtility.SetDirty(settingTarget);
                }
                GUILayout.EndHorizontal();

                // Display the good inspector for edit the config
                switch (settingTarget.EditorConfigs[selectedConfigIndex].SaveSystemType)
                {
                    case SaveSystems.Types.PersistentDataPathFile:
                        Config.EditorPersistentDataPathFileInspector(ref settingTarget, selectedConfigIndex);
                        break;
                }
            }
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        public static void DrawSpace(int _height = 20)
        {
            GUILayoutUtility.GetRect(GUIContent.none, Styles.kToggle, new GUILayoutOption[] { GUILayout.MinWidth(Styles.kMinToggleWidth), GUILayout.MaxWidth(Styles.kMaxToggleWidth), GUILayout.Height(_height) });
        }

        public static void DrawLine(int _thickness = 1, int _topPadding = 20, int _bottomPadding = 20)
        {
            DrawLine(!EditorGUIUtility.isProSkin ? Color.black : GUI.backgroundColor * 0.7058f, _thickness, _topPadding, _bottomPadding);
        }

        public static void DrawLine(Color _color, int _thickness = 1, int _topPadding = 20, int _bottomPadding = 20)
        {
            Rect bRect = GUILayoutUtility.GetRect(GUIContent.none, Styles.kToggle, new GUILayoutOption[] { GUILayout.Height(_thickness + _topPadding + _bottomPadding), GUILayout.MinWidth(Styles.kMinToggleWidth) });
            bRect.height = _thickness;
            bRect.y += _topPadding;
            EditorGUI.DrawRect(bRect, _color);
        }

        public static bool IsObsolete(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (ObsoleteAttribute[])
                fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return (attributes != null && attributes.Length > 0);
        }

        public static List<T> MaskToList<T>(List<T> _list, int _mask)
        {
            List<T> returnValues = new List<T>(); ; 
            for (int i = 0; i < _list.Count; i++)
                if ((_mask & (1 << i)) == (1 << i)) returnValues.Add(_list[i]);
            return returnValues;
        }

        public static int ListToMask<T>(List<T> _list, List<T> _listSelected)
        {
            int returnValue = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                if (_listSelected.Contains(_list[i]))
                    returnValue |= 1 << i;
            }
            return returnValue;
        }

        #endregion
    }
}