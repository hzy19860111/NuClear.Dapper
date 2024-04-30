using System.Xml.Linq;

namespace NuClear.Dapper.SqlResources
{
    public interface IXmlFileSqlResource<T> : ISqlResource<T>
            where T : IEntity
    {
        XElement Root { get; }
    }
}
