using SCSS.QueueEngine.QueueRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.QueueEngine.QueueEngines
{
    public interface IQueueEngineFactory
    {
        #region Queue Repositories

        IQueueRepository<string> StringEngineRepo { get; }

        #endregion
    }
}
