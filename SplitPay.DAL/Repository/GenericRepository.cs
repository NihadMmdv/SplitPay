using Microsoft.EntityFrameworkCore;
using SplitPay.DAL.Data;
using SplitPay.DAL.Models;
using SplitPay.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitPay.DAL.Repository
{
    public class GenericRepository<T>:IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly CustomDBContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(CustomDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        public async Task<T> Get(int id, params string[] includes)
        {
            var query = _dbSet.Where(o => o.Id == id).AsQueryable();
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IQueryable<T>> GetAll()
        {

            return _dbSet;
        }
        public T Remove(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            return entity;
        }
        public T Update(T entity)
        {

            _dbSet.Update(entity);
            return entity;
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
