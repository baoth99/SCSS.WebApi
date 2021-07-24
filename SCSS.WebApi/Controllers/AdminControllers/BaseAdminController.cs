using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Utilities.Constants;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [Route(ApplicationRestfulApi.BaseApiUrl)]
    [Produces(ApplicationRestfulApi.ApplicationProduce)]
    [ApiController]
    [Authorize(Policy = SystemPolicy.AdminPolicy)]
    public class BaseAdminController : ControllerBase
    {
    }
}
