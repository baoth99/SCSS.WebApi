using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SCSS.Utilities.Constants;

namespace SCSS.WebApi.SignalR.CollectorHubs
{
    [Authorize(Policy = SystemPolicy.CollectorPolicy, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseCollectorHub<T> : Hub<T> where T : class
    {
    }
}
