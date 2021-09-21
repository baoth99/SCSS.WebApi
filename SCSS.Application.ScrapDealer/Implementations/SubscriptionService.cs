using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class SubscriptionService : BaseService, ISubscriptionService
    {
        public SubscriptionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IFCMService fcmService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {

        }
    }
}
