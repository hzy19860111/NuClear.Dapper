using System.Collections.Generic;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class ChooseSqlNode : ISqlNode
    {
        private readonly List<ISqlNode> _ifSqlNodes;
        private readonly ISqlNode _defaultSqlNode;

        public ChooseSqlNode(List<ISqlNode> ifSqlNodes, ISqlNode defaultSqlNode)
        {
            this._ifSqlNodes = ifSqlNodes;
            this._defaultSqlNode = defaultSqlNode;
        }

        public bool Apply(DynamicContext context)
        {
            foreach (var sqlNode in _ifSqlNodes)
            {
                if (sqlNode.Apply(context))
                {
                    return true;
                }
            }

            if (_defaultSqlNode != null)
            {
                _defaultSqlNode.Apply(context);
                return true;
            }
            return false;
        }
    }
}
