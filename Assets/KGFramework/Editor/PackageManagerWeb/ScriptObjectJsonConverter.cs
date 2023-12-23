using System.IO;
using LitJson;
using UnityEditor;

namespace KG.Framework
{
    public class ScriptObjectJsonConverter
    {
        [MenuItem("Assets/KGFramework/序列化为Json", priority = 0)]
        public static void Trans2Json()
        {
            var selectedObject = Selection.activeObject;
            // if (selectedObject != null && selectedObject.GetType() == typeof(PackageInfoDetail))
            if (selectedObject != null)
            {
                string jsonSavePath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                if (!string.IsNullOrEmpty(jsonSavePath))
                {
                    jsonSavePath = $"{jsonSavePath}/package.json";
                    var jsonContent = JsonMapper.ToJson(selectedObject);
                    using(var stream = new StreamWriter(jsonSavePath))
                    {
                        stream.Write(jsonContent);
                    }
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                EditorUtility.DisplayDialog("错误提示", "请选中类型为ScriptObject.asset的物体!", "OK");
            }
        }
        
        [MenuItem("Assets/KGFramework/反序列化为ScriptableObject", priority = 1)]
        public static void Trans2ScriptableObject()
        {
            // if (!File.Exists(JsonPath)) return;
            // using(var stream = new StreamReader(JsonPath))
            // {
            //     var jsonStr = stream.ReadToEnd();
            //     var scriptableObj = JsonMapper.ToObject<TestScriptableObj>(jsonStr);
            //     AssetDatabase.CreateAsset(scriptableObj, TransScritptableObjectPath);
            //     AssetDatabase.Refresh();
            // }
        }
    }
}
