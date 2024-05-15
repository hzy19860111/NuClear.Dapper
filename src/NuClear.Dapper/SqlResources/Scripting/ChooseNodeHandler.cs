using NuClear.Dapper.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class ChooseNodeHandler : INodeHandler
    {
        public void HandleNode(XElement nodeToHandle, List<ISqlNode> targetContents)
        {
            List<ISqlNode> whenSqlNodes = new List<ISqlNode>();
            List<ISqlNode> otherwiseSqlNodes = new List<ISqlNode>();
            HandleWhenOtherwiseNodes(nodeToHandle, whenSqlNodes, otherwiseSqlNodes);
            ISqlNode defaultSqlNode = GetDefaultSqlNode(otherwiseSqlNodes);
            ChooseSqlNode chooseSqlNode = new ChooseSqlNode(whenSqlNodes, defaultSqlNode);
            targetContents.Add(chooseSqlNode);
        }

        private void HandleWhenOtherwiseNodes(XNode chooseSqlNode, List<ISqlNode> ifSqlNodes, List<ISqlNode> defaultSqlNodes)
        {
            var children = XElement.Parse(chooseSqlNode.ToString()).Elements();
            foreach (var child in children)
            {
                string nodeName = child.Name.LocalName;
                INodeHandler handler = XElementExtensions.GetNodeHandler(nodeName);
                if (handler is IfNodeHandler ifHanler)
                {
                    ifHanler.HandleNode(child, ifSqlNodes);
                }
                else if (handler is OtherwiseNodeHandler otherwiseHanler)
                {
                    otherwiseHanler.HandleNode(child, defaultSqlNodes);
                }
            }
        }

        private ISqlNode GetDefaultSqlNode(List<ISqlNode> defaultSqlNodes)
        {
            ISqlNode defaultSqlNode = null;
            if (defaultSqlNodes.Count == 1)
            {
                defaultSqlNode = defaultSqlNodes.First();
            }
            else if (defaultSqlNodes.Count > 1)
            {
                throw new SqlParseException("Choose节点下存在多个otherwise节点！");
            }
            return defaultSqlNode;
        }
    }
}
