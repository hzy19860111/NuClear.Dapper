using System.Collections.Generic;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class MixedSqlNode : ISqlNode
    {
        private readonly List<ISqlNode> _contents;

        public MixedSqlNode(List<ISqlNode> contents)
        {
            this._contents = contents;
        }

        public bool Apply(DynamicContext context)
        {
            _contents.ForEach(n => n.Apply(context));
            return true;
        }
    }
}
