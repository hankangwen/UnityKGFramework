using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

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
        private const string IPAddress = "https://raw.githubusercontent.com/hankangwen/UnityKGFramework/main/PackageManagerWeb";
        //父目录
        private const string ParentFolder = "Library/com.kgframeroek.packagemanager";
        //manifest.json文件
        private static string Manifest => $"{ParentFolder}/manifest.json";
        //资源包列表
        private List<PackageInfoDetail> _packages;
        //资源包字典
        private Dictionary<string, List<PackageInfoDetail>> _dict;
        //折叠栏字典
        private Dictionary<string, bool> _foldout;
        //用于存储资源包版本是否可升级的字典
        private Dictionary<string, bool> _updatable;
        //检索内容
        private string _searchContent;
        //左侧内容滚动
        private Vector2 _leftScroll;
        //右侧内容滚动
        private Vector2 _rightScroll;
        //用于存储最新更新时间的Key值
        private const string LastUpdateTimeKey = "Packages Last Update Time";
        //最新更新时间
        private string _lastUpdateTime;
        //当前选中的资源包
        private PackageInfoDetail _selectedPackage;
        //标题样式
        private GUIStyle _titleStyle;
        //版本样式
        private GUIStyle _versionStyle;
        //粗体样式
        private GUIStyle _boldLabelStyle;
        //依赖引用列表样式
        private GUIStyle _dependenciesStyle;
        //依赖项折叠栏
        private bool _dependenciesFoldOut = true;
        
        private void OnEnable()
        {
            //如果不存在manafest.json文件
            //直接发起网络请求获取资源包信息
            if (!File.Exists(Manifest))
            {
                UpdatePackagesInfo();
            }
            else
            {
                Build();
            }
        }

        /// <summary>
        /// 刷新资源包信息
        /// </summary>
        private void UpdatePackagesInfo()
        {
            //URL
            string url = $"{IPAddress}/manifest.json";
            //发起网络请求
            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();
            using (Stream stream = webResponse.GetResponseStream())
            {
                try
                {
                    if (!Directory.Exists(ParentFolder)) Directory.CreateDirectory(ParentFolder);
                    using (FileStream fs = new FileStream(Manifest, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[1024];
                        int count = 0;
                        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, count);
                        }
                        //刷新最新更新时间并存储
                        _lastUpdateTime = $"Last Update {DateTime.Now}";
                        EditorPrefs.SetString(LastUpdateTimeKey, _lastUpdateTime);
                        //重置
                        _selectedPackage = null;
                    }
                }
                finally
                {
                    Build();
                }
            }
        }

        private void Build()
        {
            //用于存储所有PackageAttribute属性
            List<PackageAttribute> attributes = new List<PackageAttribute>();
            //获取所有类型 遍历判断是否包含PackageAttribute属性
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                foreach (var type in assemblyTypes)
                {
                    var attribute = type.GetCustomAttribute<PackageAttribute>();
                    if (attribute != null)
                    {
                        attributes.Add(attribute);
                    }
                }
            }
            
            //读取文件 通过json方式
            string jsonString = File.ReadAllText(Manifest);
            PackageInfoDetailList packageList = JsonUtility.FromJson<PackageInfoDetailList>(jsonString);
            _packages = packageList.packageList;
            
            //初始化字典
            _dict = new Dictionary<string, List<PackageInfoDetail>>();
            _foldout = new Dictionary<string, bool>();
            _updatable = new Dictionary<string, bool>();
            //遍历资源包列表
            for (int i = 0; i < _packages?.Count; i++)
            {
                var package = _packages[i];
                //判断是否已经有相应的资源包信息 表示已经安装
                var target = attributes.Find(m => m.ToString() == package.ToString());
                package.isInstalled = target != null;
                //如果包含依赖项
                if (package.dependencies != null && package.dependencies.Length > 0)
                {
                    //遍历依赖项 查找其是否已经安装
                    for (int j = 0; j < package.dependencies.Length; j++)
                    {
                        var dp = package.dependencies[j];
                        var dpTarget = attributes.Find(m => m.ToString() == dp.ToString());
                        dp.isInstalled = dpTarget != null;
                    }
                }
            }
            //再次遍历 获取引用关系
            for (int i = 0; i < _packages?.Count; i++)
            {
                var package = _packages[i];
                package.referencies = _packages.Where(m => m.dependencies != null
                    && Array.Find(m.dependencies, m => m.ToString() == package.ToString()) != null) .ToArray();
                //填充字典
                if (!_dict.ContainsKey(package.packageName))
                {
                    _dict.Add(package.packageName, new List<PackageInfoDetail>());
                    _foldout.Add(package.packageName, false);
                    _updatable.Add(package.packageName, false);
                }
                _dict[package.packageName].Add(package);
            }
            //遍历字典 进行排序
            //遍历字典 进行排序
            foreach (var kv in _dict)
            {
                var list = kv.Value;
                list = list.OrderByDescending(m => m.version).ToList();
                //判断是否有可升级版本
                _updatable[kv.Key] = list.Count > 1 && list.OrderBy(m => m.isInstalled).ToList()[0] != list.OrderBy(m => m.version).ToList()[0]
                                                    && list.Find(m => m.isInstalled) != null && list.Find(m => m.isInstalled) != list[0];
            }
        }
        
        private void OnGUI()
        {
            if (_titleStyle == null) StyleInit();
            
            
        }
        
        //样式初始化
        private void StyleInit()
        {
            
        }

        private void OnTopGUI()
        {
            
        }
        
        private void OnLeftGUI()
        {
            
        }

        private void OnRightGUI()
        {
            
        }
    }
}