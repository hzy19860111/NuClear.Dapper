using System.Collections.Generic;
using System.Xml.Linq;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class OtherwiseNodeHandler : INodeHandler
    {
        public void HandleNode(XElement nodeToHandle, List<ISqlNode> targetContents)
        {
            MixedSqlNode mixedSqlNode = nodeToHandle.ParseDynamicTags();
            targetContents.Add(mixedSqlNode);
        }
    }
}
