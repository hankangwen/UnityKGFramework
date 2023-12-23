using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace KG.Framework
{
    /// <summary>
    /// 通过KGFramework/Packages下的子目录中的package.json，创建manifest.json文件到PackageManagerWeb
    /// </summary>
    public class PackageManagerWeb : EditorWindow
    {
        [MenuItem("KGFramework/Package Manager Web", priority = 1000)]
        public static void Open()
        {
            var window = GetWindow<PackageManagerWeb>();
            window.titleContent = EditorGUIUtility.TrTextContentWithIcon("PackageManagerWeb", "Package Manager");
            window.minSize = new Vector2(605f, 300f);
            window.Show();
        }

        private const string TargetManifest = "../PackageManagerWeb/manifest.json";
        private const string PackageJsonListSaveKey = "PackageJsonListSaveKey";
        private List<string> _packageJsonList = new List<string>();

        private void OnEnable()
        {
            string json = EditorPrefs.GetString(PackageJsonListSaveKey, string.Empty);
            if (json != string.Empty)
            {
                //反序列化
                _packageJsonList = JsonMapper.ToObject<List<string>>(json);
            }
        }

        private void OnDisable()
        {
            if (_packageJsonList.Count >= 0)
            {
                //序列化
                string json = JsonMapper.ToJson(_packageJsonList);
                EditorPrefs.SetString(PackageJsonListSaveKey, json);
            }
        }

        private void OnGUI()
        {
            OnTopGUI();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create New"))
            {
                _packageJsonList.Add(string.Empty);
            }

            if (GUILayout.Button("Create TargetManifest.json"))
            {
                CreateTargetManifestJson();
            }
        }

        private void OnTopGUI()
        {
            if (_packageJsonList.Count <= 0) return;

            GUILayout.BeginVertical();
            {
                for (int i = 0; i < _packageJsonList.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("package.json path:", EditorStyles.boldLabel, GUILayout.Width(120));
                        _packageJsonList[i] = EditorGUILayout.TextField(_packageJsonList[i], GUILayout.Width(300));
                        if (GUILayout.Button("Select .json", GUILayout.Width(90)))
                        {
                            _packageJsonList[i] = EditorUtility
                                .OpenFilePanel("Select Json File", _packageJsonList[i], "json")
                                .Substring(Application.dataPath.Length - "Assets".Length);
                        }

                        if (GUILayout.Button("Remove", GUILayout.Width(80)))
                        {
                            _packageJsonList.RemoveAt(i);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

        private void CreateTargetManifestJson()
        {
        }
    }
}