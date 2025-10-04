using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DataAccess.Reposioties
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext dbContext;
        private readonly DbSet<T> table;

        public GenericRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            table= this.dbContext.Set<T>();  
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
            // dbContext.Set<T>().Skip(5).Take(5).InClude(p=> p.ProductBrand).InClude(p=> p.ProductType).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
            //dbContext.Set<T>().Where(p=> p.Id== id).InClude(p => p.ProductBrand).InClude(p => p.ProductType).FirstOrDefaultAsync();
        }
        public async Task<long> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).LongCountAsync();
        }
        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(table,spec);
            // dbContext.Set<T>();
        }

        public async Task Add(T entity)
            => await table.AddAsync(entity);

        public void Update(T entity)
            => table.Update(entity);
        //=> dbContext.Entry(entity).State = EntityState.Modified;

        public void Delete(T entity)
            => table.Remove(entity);
    }
}
