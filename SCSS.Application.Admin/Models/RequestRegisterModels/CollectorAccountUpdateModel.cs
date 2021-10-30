using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.RequestRegisterModels
{
    public class CollectorAccountUpdateModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int Gender { get; set; }

        public string BirthDate { get; set; }

        public string IDCard { get; set; }

        public string ImageUrl { get; set; }

    }
}
