using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Models;
using Portfolio.Core.Specifications;
using Portfolio.Repository.Data;
using Portfolio.Repository.Specifications;
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
            //if (typeof(T) == typeof(Design))
            //{
            //    var data = await _context.Set<Design>().Include(d => d.Category).ToListAsync();
            //    return data as List<T>;
            //}


            //if (typeof(T) == typeof(Category))
            //{
            //    var data = await _context.Set<Category>().Include(d => d.Designs).ToListAsync();
            //    return data as List<T>;
            //}






            return await _context.Set<T>().ToListAsync();
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



        public async Task<T?> GetWithSpecByIdAsync(ISpecifications<T> spec)
        {
           return await SpecificationsEvaluators<T>.GetQuery(_context.Set<T>() , spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await SpecificationsEvaluators<T>.GetQuery(_context.Set<T>(), spec).ToListAsync();
        }










        public async Task<T> AddAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
            return entity;
        }





        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }


        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

       
    }
}
