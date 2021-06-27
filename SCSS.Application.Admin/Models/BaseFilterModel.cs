using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models
{
    public class BaseFilterModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string SortField { get; set; }

        public bool IsSortDesc { get; set; }
    }
}
