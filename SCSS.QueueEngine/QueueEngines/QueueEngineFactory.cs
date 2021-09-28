using SCSS.QueueEngine.QueueEngines;
using SCSS.QueueEngine.QueueRepositories;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.QueueEngine.QueueEngines
{
    public class QueueEngineFactory : IQueueEngineFactory
    {
        #region Private variable Engine

        // TODO:
        private IQueueRepository<string> _stringEngineRepo;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueEngineFactory"/> class.
        /// </summary>
        public QueueEngineFactory()
        {
        }

        #endregion

        #region Publish Access Engine

        // TODO: 
        public IQueueRepository<string> StringEngineRepo
                => _stringEngineRepo ??= (_stringEngineRepo = new QueueRepository<string>(new ConcurrentQueue<string>()));


        #endregion Publish Access Engine
    }
}
