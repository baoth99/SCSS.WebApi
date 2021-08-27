using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class SwaggerDefinition
    {
        public static readonly List<SwaggerDocApiInfo> Swagger = new List<SwaggerDocApiInfo>()
        {
            new SwaggerDocApiInfo()
            {
                Title = "SCSS.WebApi.Admin",
                Version = "v1",
                TermsOfService = new Uri("https://github.com/Baoth99"),
                ContactEmail = "baoth799@gmail.com",
                ContactName = "Trần Hoài Bảo",
                LicenseName = "FPT University",
                LicenseUrl = new Uri("https://hcmuni.fpt.edu.vn/"),
                Description = "SCSS.WebApi for Admin Role",
                UrlDefination = "/swagger/v1/swagger.json"
            },
            new SwaggerDocApiInfo()
            {
                Title = "SCSS.WebApi.Seller",
                Version = "v2",
                TermsOfService = new Uri("https://github.com/Baoth99"),
                ContactEmail = "baoth799@gmail.com",
                ContactName = "Trần Hoài Bảo",
                LicenseName = "FPT University",
                LicenseUrl = new Uri("https://hcmuni.fpt.edu.vn/"),
                Description = "SCSS.WebApi for Seller Role",
                UrlDefination = "/swagger/v2/swagger.json"
            },
            new SwaggerDocApiInfo()
            {
                Title = "SCSS.WebApi.Dealer",
                Version = "v3",
                TermsOfService = new Uri("https://github.com/Baoth99"),
                ContactEmail = "baoth799@gmail.com",
                ContactName = "Trần Hoài Bảo",
                LicenseName = "FPT University",
                LicenseUrl = new Uri("https://hcmuni.fpt.edu.vn/"),
                Description = "SCSS.WebApi for Dealer Role",
                UrlDefination = "/swagger/v3/swagger.json"
            },
            new SwaggerDocApiInfo()
            {
                Title = "SCSS.WebApi.Collector",
                Version = "v4",
                TermsOfService = new Uri("https://github.com/Baoth99"),
                ContactEmail = "baoth799@gmail.com",
                ContactName = "Trần Hoài Bảo",
                LicenseName = "FPT University",
                LicenseUrl = new Uri("https://hcmuni.fpt.edu.vn/"),
                Description = "SCSS.WebApi for Collector Role",
                UrlDefination = "/swagger/v4/swagger.json"
            }
        };



    }


    public class SwaggerDocApiInfo
    {
        public string Version { get; set; }

        public string Title { get; set; }

        public Uri TermsOfService { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string LicenseName { get; set; }

        public Uri LicenseUrl { get; set; }

        public string Description { get; set; }

        public string UrlDefination { get; set; }
    }


}
