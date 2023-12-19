using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace KG.Framework
{
    /// <summary>
    /// 资源包管理器
    /// </summary>
    public class PackageManager : EditorWindow
    {
        [MenuItem("KGFramework/Package Manager", priority = 999)]
        public static void Open()
        {
            var window = GetWindow<PackageManager>();
            window.titleContent = EditorGUIUtility.TrTextContentWithIcon("Package Manager", "Package Manager");
            window.minSize = new Vector2(600f, 300f);
            window.Show();
        }

        //服务器IP地址
        private const string IPAddress = "https://github.com/hankangwen/UnityKGFramework/PackageManagerWeb";
        //manifest.json文件
        private const string Manifest = "Library/com.kgframeroek.packagemanager/manifest.dat";

        private void OnEnable()
        {
            //如果不存在manafest.json文件
            //直接发起网络请求获取资源包信息
            if (!File.Exists(Manifest))
            {
                
            }
            else
            {
                
            }
        }

        /// <summary>
        /// 刷新资源包信息
        /// </summary>
        private void UpdatePackagesInfo()
        {
            //URL
            string url = $"{IPAddress}/manifest.dat";
            //发起网络请求
            
        }
        
        
    }
}
