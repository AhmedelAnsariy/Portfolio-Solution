using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Models
{
    public class EvaluationMonthly
    {
        public bool ActiveRate { get; set; }


        public string PersonName { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Rate { get; set; }

       public string? BackupQuestion { get; set; }
        public string Description { get; set; }




    }
}
