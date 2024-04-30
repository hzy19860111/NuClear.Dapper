using System.Collections.Generic;
using System.Xml.Linq;

namespace NuClear.Dapper.SqlResources.Scripting
{
    internal interface INodeHandler
    {
        void HandleNode(XElement nodeToHandle, List<ISqlNode> targetContents);
    }
}
