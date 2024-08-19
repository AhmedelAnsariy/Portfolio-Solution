using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Models
{
    public class Design : BaseClass
    {
        public string? Name { get; set; }

        public string PictureUrl { get; set; }

        public string? Link { get; set; }



        public Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}
