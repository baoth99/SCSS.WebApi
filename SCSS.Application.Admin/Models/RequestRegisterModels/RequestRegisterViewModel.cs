using System;

namespace SCSS.Application.Admin.Models.RequestRegisterModels
{
    public class CollectorRequestRegisterViewModel
    {
        public Guid Id { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public int? Gender { get; set; }

        public string RegisterTime { get; set; }

        public int? Status { get; set; }
    }


    public class DealerRequestRegisterViewModel
    {
        public Guid Id { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string DealerName { get; set; }

        public int? Gender { get; set; }

        public string RegisterTime { get; set; }

        public string ManagedBy { get; set; }

        public int? Status { get; set; }
    }
}
