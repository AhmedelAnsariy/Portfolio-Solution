using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Portfolio.Core.Models;
using Portfolio.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Repository.Specifications
{
    public class SpecificationsEvaluators<TEntity> where TEntity : BaseClass
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
        {
            var query = inputQuery;  

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            } 

            query = spec.Includes.Aggregate(query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            return query;
        }

    }
}
