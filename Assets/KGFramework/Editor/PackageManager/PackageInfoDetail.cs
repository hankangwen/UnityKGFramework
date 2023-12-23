using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KG.Framework
{
    /// <summary>
    /// 资源包详细信息列表
    /// </summary>
    [Serializable]
    public class PackageInfoDetailList
    {
        public List<PackageInfoDetail> packageList;
    }
    
    
    /// <summary>
    /// 资源包详细信息
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "PackageJson", menuName = "KGFramework/CreatePackageJson")]
    public class PackageInfoDetail : PackageInfoBrief
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string author;

        /// <summary>
        /// 介绍
        /// </summary>
        public string description;

        /// <summary>
        /// 下载地址
        /// </summary>
        public string downloadUrl;

        /// <summary>
        /// 文档地址
        /// </summary>
        public string documentationUrl;

        /// <summary>
        /// 发布日期
        /// </summary>
        public string releaseDate;

        /// <summary>
        /// 依赖项
        /// </summary>
        public PackageInfoBrief[] dependencies;

        /// <summary>
        /// 被引用
        /// </summary>
        public PackageInfoBrief[] referencies;
    }
}