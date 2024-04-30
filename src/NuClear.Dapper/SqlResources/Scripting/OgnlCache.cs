using ognl;
using System;
using System.Collections.Concurrent;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class OgnlCache
    {
        private static readonly DefaultMemberAccess _memberAccess = new DefaultMemberAccess(true);
        private static readonly DefaultClassResolver _classResolver = new DefaultClassResolver();
        private static readonly DefaultTypeConverter _typeConverter = new DefaultTypeConverter();
        private static readonly ConcurrentDictionary<string, object> _expressionCache = new ConcurrentDictionary<string, object>();

        private OgnlCache()
        {
            // Prevent Instantiation of Static Class
        }

        public static object GetValue(string expression, object root)
        {
            try
            {
                var context = Ognl.createDefaultContext(root, _classResolver, _typeConverter, _memberAccess);
                return Ognl.getValue(ParseExpression(expression), context, root);
            }
            catch (OgnlException e)
            {
                throw new Exception("Error evaluating expression '" + expression + "'. Cause: " + e, e);
            }
        }

        private static object ParseExpression(string expression)
        {
            _expressionCache.TryGetValue(expression, out var node);
            if (node == null)
            {
                node = Ognl.parseExpression(expression);
                _expressionCache.TryAdd(expression, node);
            }

            return node;
        }
    }
}
