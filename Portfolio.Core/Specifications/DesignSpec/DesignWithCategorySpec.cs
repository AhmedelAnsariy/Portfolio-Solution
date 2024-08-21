using Portfolio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Specifications.DesignSpec
{
    public class DesignWithCategorySpec : BaseSpecification<Design>
    {

        public DesignWithCategorySpec() : base()
        {
            Includes.Add(d => d.Category);
        }

        public DesignWithCategorySpec(int id ) : base( d=> d.Id == id)
        {
            Includes.Add(d => d.Category);
        }

    }
}
