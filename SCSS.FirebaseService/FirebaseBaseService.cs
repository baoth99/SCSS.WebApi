using SCSS.AWSService.Interfaces;

namespace SCSS.FirebaseService
{
    public class FirebaseBaseService
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        protected ILoggerService Logger { get; private set; }


        public FirebaseBaseService(ILoggerService logger)
        {
            Logger = logger;
        }
    }
}
