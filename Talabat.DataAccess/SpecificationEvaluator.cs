using Talabat.Core.Entities;
using Talabat.Core.Specifications;
using Microsoft.EntityFrameworkCore;


namespace Talabat.DataAccess
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery; // dbContext.Set<Product>();

            if(specification.Criteria is not null) // Criteria P=> p.Id==1
                query= query.Where(specification.Criteria);
            // dbContext.Set<Product>().Where(P=> p.Id==1);

            if(specification.OrderBy is not null)
                query= query.OrderBy(specification.OrderBy);
            // dbContext.Set<Product>().OrderBy(p=> p.Price);

            if (specification.OrderByDesc is not null)
                query = query.OrderByDescending(specification.OrderByDesc);
            // dbContext.Set<Product>().OrderByDescending(p => p.Price);

            if(specification.IsPaginationEnabled)
                query= query.Skip(specification.Skip).Take(specification.Take);

            query = specification.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            // dbContext.Set<Product>().Where(P=> p.Id==1).Include(P=> P.ProductBrand).Include(P=> P.ProductType);

            return query;
        }
    }
}
