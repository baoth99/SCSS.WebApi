using System;


namespace SCSS.Application.Admin.Models.RequestRegisterModels
{
    public class CollectorRequestRegisterViewDetailModel
    {
        public Guid Id { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string BirthDate { get; set; }

        public int? Status { get; set; }

        public int? Gender { get; set; }

        public string Address { get; set; }

        public string IDCard { get; set; }

        public string RegisterTime { get; set; }

    }
}
