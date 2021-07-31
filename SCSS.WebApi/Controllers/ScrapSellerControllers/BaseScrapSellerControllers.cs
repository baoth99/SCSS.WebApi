using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Utilities.Constants;


namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [Route(ApplicationRestfulApi.BaseApiUrl)]
    [Produces(ApplicationRestfulApi.ApplicationProduce)]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    [Authorize(Policy = SystemPolicy.SellerPolicy)]
    public class BaseScrapSellerControllers : ControllerBase
    {
    }
}
