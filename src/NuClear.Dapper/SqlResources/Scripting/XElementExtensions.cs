using NuClear.Dapper.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public static class XElementExtensions
    {
        private static readonly Dictionary<string, INodeHandler> _nodeHandlerMap = new Dictionary<string, INodeHandler>();

        static XElementExtensions()
        {
            InitNodeHandlerMap();
        }

        internal static INodeHandler GetNodeHandler(string nodeName)
        {
            return _nodeHandlerMap[nodeName];
        }

        private static void InitNodeHandlerMap()
        {
            _nodeHandlerMap.Add("if", new IfNodeHandler());
            _nodeHandlerMap.Add("choose", new ChooseNodeHandler());
            _nodeHandlerMap.Add("otherwise", new OtherwiseNodeHandler());
            _nodeHandlerMap.Add("when", new IfNodeHandler());
        }

        public static MixedSqlNode ParseDynamicTags(this XElement node)
        {
            List<ISqlNode> contents = new List<ISqlNode>();

            var children = node.Nodes().ToList();
            for (int i = 0; i < children.Count; i++)
            {
                XNode child = children[i];
                if (child.NodeType == System.Xml.XmlNodeType.Text || child.NodeType == System.Xml.XmlNodeType.CDATA)
                {
                    TextSqlNode textSqlNode = new TextSqlNode(((System.Xml.Linq.XText)child).Value);
                    contents.Add(textSqlNode);
                }
                else if (child.NodeType == System.Xml.XmlNodeType.Element)
                {
                    var elementNode = XElement.Parse(child.ToString());
                    var nodeName = elementNode.Name.ToString();
                    INodeHandler handler = GetNodeHandler(nodeName) ?? throw new SqlParseException($"不识别的ElementName：{nodeName}!");
                    handler.HandleNode(elementNode, contents);
                }
            }
            return new MixedSqlNode(contents);
        }
    }
}
