using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataDbContext _context;
        private Hashtable _repositories; 

        public UnitOfWork(DataDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }






        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }




        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseClass
        {
            var type = typeof(TEntity).Name;
            if(!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenericRepository<TEntity>;
            
        }
    }
}



