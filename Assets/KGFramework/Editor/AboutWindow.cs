using UnityEditor;
using UnityEngine;

namespace KG.Framework
{
    public class AboutWindow : EditorWindow
    {
        #region 自动弹出

        private const string HideAutoShow = "KGFramework_HideAutoShowAbout";

        public static bool HideAutoShowAbout
        {
            get => EditorPrefs.GetBool(HideAutoShow, false);
            set => EditorPrefs.SetBool(HideAutoShow, value);
        }

        [InitializeOnLoadMethod]
        private static void OnEditorLaunch()
        {
            if (!HideAutoShowAbout && EditorApplication.timeSinceStartup < 30)
            {
                EditorApplication.delayCall += Open;
            }
        }

        #endregion


        [MenuItem("KGFramework/About", priority = 0)]
        private static void Open()
        {
            var window = GetWindow<AboutWindow>(true, "About", true);
            window.position = new Rect(200, 200, 360, 300);
            window.minSize = new Vector2(360, 300);
            window.maxSize = new Vector2(360, 300);
            window.Show();
        }

        private const string CSDNUrl = "https://blog.csdn.net/qq_34035956";
        private const string GithubUrl = "https://github.com/KervenGame";
        private const string QQAccount = "1397796710";
        private const string Email = "1397796710@qq.com";

        private void OnGUI()
        {
            GUILayout.Label("KGFramework", new GUIStyle(GUI.skin.label) { fontSize = 50, fontStyle = FontStyle.Bold });

            GUILayout.Label("Version: 1.0.0");
            GUILayout.Label("本框架开发所用环境: Unity2022.3.14f1");
            GUILayout.Label("请将KGFramework直接放在Assets根目录下使用");

            GUILayout.Space(20f);

            GUILayout.Label("作者：KervenGame");

            GUILayout.BeginHorizontal();
            GUILayout.Label($"CSDN主页：{CSDNUrl}");
            if (GUILayout.Button("访问", GUILayout.Width(40f)))
            {
                Application.OpenURL(CSDNUrl);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"GitHub主页：{GithubUrl}");
            if (GUILayout.Button("访问", GUILayout.Width(40f)))
            {
                Application.OpenURL(GithubUrl);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"QQ号：{QQAccount}");
            if (GUILayout.Button("复制", GUILayout.Width(40f)))
            {
                GUIUtility.systemCopyBuffer = QQAccount;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Email：{Email}");
            if (GUILayout.Button("复制", GUILayout.Width(40f)))
            {
                GUIUtility.systemCopyBuffer = Email;
            }

            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            {
                HideAutoShowAbout = EditorGUILayout.Toggle("取消自动弹出", HideAutoShowAbout);
            }
            GUILayout.EndHorizontal();
        }
    }
}