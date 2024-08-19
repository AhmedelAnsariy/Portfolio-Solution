using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Repository.Repositories
{
      public class GenericRepository<T> : IGenericRepository<T> where T : BaseClass
    {
        private readonly DataDbContext _context;

        public GenericRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
           var data = await _context.Set<T>().ToListAsync();
            return data;
        }



        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} was not found.");
            }
            return entity;
        }








        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }
        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
