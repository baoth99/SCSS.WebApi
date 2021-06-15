using SCSS.Data.EF.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin
{
    internal class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; private set; }


        #region Constructor

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #endregion

    }
}
