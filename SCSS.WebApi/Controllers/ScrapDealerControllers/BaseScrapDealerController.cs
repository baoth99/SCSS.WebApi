using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Utilities.Constants;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [Route(ApplicationRestfulApi.BaseApiUrl)]
    [Produces(ApplicationRestfulApi.ApplicationProduce)]
    [ApiExplorerSettings(GroupName = "v3")]
    [ApiController]
    [Authorize(Policy = SystemPolicy.DealerPolicy)]
    public class BaseScrapDealerController : ControllerBase
    {
    }
}
