using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuClear.Dapper.QueryObject
{
    public interface ISearch
    {
        Query CreateQuery();
    }
}
