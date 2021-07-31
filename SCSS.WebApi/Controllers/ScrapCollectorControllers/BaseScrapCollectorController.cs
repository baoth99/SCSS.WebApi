using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Utilities.Constants;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [Route(ApplicationRestfulApi.BaseApiUrl)]
    [Produces(ApplicationRestfulApi.ApplicationProduce)]
    [ApiExplorerSettings(GroupName = "v4")]
    [ApiController]
    [Authorize(Policy = SystemPolicy.CollectorPolicy)]
    public class BaseScrapCollectorController : ControllerBase
    {
    }
}
