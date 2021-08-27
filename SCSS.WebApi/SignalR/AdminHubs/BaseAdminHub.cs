using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SCSS.Utilities.Constants;

namespace SCSS.WebApi.SignalR.AdminHubs
{
    [Authorize(Policy = SystemPolicy.AdminPolicy, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseAdminHub<T> : Hub<T> where T : class
    {
    }
}
