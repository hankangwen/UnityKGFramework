using System;
using LitJson;
using UnityEngine;

namespace KG.Framework
{
    /// <summary>
    /// 资源包简要信息
    /// </summary>
    [Serializable]
    public class PackageInfoBrief : ScriptableObject
    {
        /// <summary>
        /// 资源包名称
        /// </summary>
        public string packageName;

        /// <summary>
        /// 资源包版本
        /// </summary>
        public string version;

        /// <summary>
        /// 是否已经安装
        /// </summary>
        [JsonIgnore]
        public bool isInstalled;
        
        public override string ToString()
        {
            return $"{packageName}-{version}";
        }
    }
}