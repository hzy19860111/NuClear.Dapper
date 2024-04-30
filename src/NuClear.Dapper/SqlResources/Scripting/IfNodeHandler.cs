using System.Collections.Generic;
using System.Xml.Linq;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class IfNodeHandler : INodeHandler
    {
        public void HandleNode(XElement nodeToHandle, List<ISqlNode> targetContents)
        {
            MixedSqlNode mixedSqlNode = nodeToHandle.ParseDynamicTags();
            string test = nodeToHandle.Attribute("test").Value;
            IfSqlNode ifSqlNode = new IfSqlNode(mixedSqlNode, test);
            targetContents.Add(ifSqlNode);
        }
    }
}
