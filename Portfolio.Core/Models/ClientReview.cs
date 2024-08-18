using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Models
{
    public class ClientReview : BaseClass
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }


    }
}
