using Microsoft.AspNetCore.Builder;
using System;
using SCSS.WebApi.SystemConstants;
using SCSS.WebApi.SignalR.CollectorHubs.Hubs;

namespace SCSS.WebApi.SystemConfigurations
{
    internal static class EndpointsConfigSetUp
    {
        public static void UseEndpointsSetUp(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentException(nameof(app));
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<CollectingRequestHub>(ScrapCollectorApiUrlDefinition.HubApiUrl.CollectingRequest);
                endpoints.MapControllers();
            });
        }
    }
}
