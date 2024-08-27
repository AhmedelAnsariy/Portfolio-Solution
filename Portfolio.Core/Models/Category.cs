using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Models
{
    public class Category : BaseClass
    { 
        public string Name { get; set; }

        public string? Code { get; set; }


        public ICollection<Design> Designs { get; set; }
    }
}
