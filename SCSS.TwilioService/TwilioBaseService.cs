using SCSS.AWSService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.TwilioService
{
    public class TwilioBaseService
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILoggerService Logger { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TwilioBaseService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TwilioBaseService(ILoggerService logger)
        {
            Logger = logger;
        }

        #endregion

    }
}
