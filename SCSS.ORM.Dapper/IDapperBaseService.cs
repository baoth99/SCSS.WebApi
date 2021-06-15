using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.ORM.Dapper
{
    public interface IDapperBaseService : IDisposable
    {
        new void Dispose();
    }
}
