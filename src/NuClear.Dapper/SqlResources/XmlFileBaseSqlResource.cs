using NuClear.Dapper.SqlResources.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace NuClear.Dapper.SqlResources
{
    public abstract class XmlFileBaseSqlResource<T> : IXmlFileSqlResource<T>
            where T : IEntity
    {

        private static readonly object lockObject = new object();

        private static readonly Dictionary<string, ISqlSource> sqlSourceCache = new Dictionary<string, ISqlSource>();


        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string FileName
        {
            get
            {
                return typeof(T).Name;
            }
        }
        /// <summary>
        /// 目录
        /// </summary>
        protected abstract string Directory { get; }

        /// <summary>
        /// 程序集名称
        /// </summary>
        protected virtual string AssemblyName { get; }

        private XElement root;
        private ISqlSource sqlSource;

        private string GetSqlSourceCacheKey()
        {
            return Path.Combine(AppContext.BaseDirectory, Directory, FileName + ".xml");
        }

        public ISqlSource SqlSource
        {
            get
            {
                if (sqlSource == null)
                {
                    lock (lockObject)
                    {
                        if (sqlSource == null)
                        {
                            var key = GetSqlSourceCacheKey();
                            if (sqlSourceCache.ContainsKey(key))
                            {
                                sqlSource = sqlSourceCache[key];
                            }
                            else
                            {
                                sqlSource = XMLScriptBuilder.ParseScriptNode(Root);
                                sqlSourceCache.Add(key, SqlSource);
                            }
                        }
                    }
                }

                return sqlSource;
            }
        }

        public virtual XElement Root
        {
            get
            {
                if (root == null)
                {
                    var path = System.IO.Path.Combine(AppContext.BaseDirectory, Directory, FileName + ".xml");
                    if (File.Exists(path))
                    {
                        root = XElement.Load(System.IO.Path.Combine(AppContext.BaseDirectory, Directory, FileName + ".xml"));
                    }
                    else if (!string.IsNullOrWhiteSpace(AssemblyName))
                    {
                        //获取程序集嵌入资源
                        var assembly = Assembly.Load(AssemblyName);
                        if (assembly == null)
                            throw new ArgumentNullException(nameof(assembly));

                        var fullName = $"{AssemblyName}.{Directory.Replace("/", ".")}.{FileName + ".xml"}";
                        using (var stream = assembly.GetManifestResourceStream(fullName))
                        {
                            root = XElement.Load(stream);
                        }
                    }
                }

                return root;
            }
        }

        public virtual string Select => GetSql(nameof(Select));

        public virtual string Insert => GetSql(nameof(Insert));

        public virtual string Update => GetSql(nameof(Update));

        public virtual string Delete => GetSql(nameof(Delete));

        public virtual string GetSql(string key)
        {
            return this.GetSql(key, null);
        }

        public virtual string GetSql(string key, object parameterObject)
        {
            return this.SqlSource.GetSql(key, parameterObject);
        }
    }
}
