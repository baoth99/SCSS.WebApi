using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.EF.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories

        // Create Repository here !

        #endregion

        Task CommitAsync();
    }
}
