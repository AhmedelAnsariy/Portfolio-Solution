using Portfolio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {

        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseClass;

        Task<int> CompleteAsync();




    }
}
