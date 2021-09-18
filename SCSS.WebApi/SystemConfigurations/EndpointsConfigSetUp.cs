using Microsoft.AspNetCore.Builder;
using SCSS.WebApi.SignalR.AdminHubs.Hubs;
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
                endpoints.MapHub<AmountOfCollectingRequestHub>(AdminApiUrlDefinition.HubApiUrl.AmountCollectingRequest);
                endpoints.MapHub<AmountOfTransactionHub>(AdminApiUrlDefinition.HubApiUrl.AmountTransaction);

                endpoints.MapHub<CollectingRequestHub>(ScrapCollectorApiUrlDefinition.HubApiUrl.CollectingRequest);
                endpoints.MapControllers();
            });
        }
    }
}
