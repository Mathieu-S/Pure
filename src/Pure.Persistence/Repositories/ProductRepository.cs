using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Pure.Application.Repositories;
using Pure.Domain.Models;

namespace Pure.Persistence.Repositories
{
    /// <inheritdoc/>
    public class ProductRepository : IProductRepository
    {
        private readonly PureDbContext _dbContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbContext">The DbContext.</param>
        /// <exception cref="ArgumentNullException">Throw when the dbContext is null.</exception>
        public ProductRepository(PureDbContext dbContext)
        {
            _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _dbContext.Products
                .Include(x => x.Brand)
                .ToArrayAsync();
        }

        /// <inheritdoc/>
        public async Task<Product> GetAsync(Guid id)
        {
            return await GetAsync(id, false);
        }

        /// <inheritdoc/>
        public async Task<Product> GetAsync(Guid id, bool asTracking)
        {
            var query = _dbContext.Products
                .Include(x => x.Brand)
                .AsQueryable();

            if (asTracking)
            {
                query = query.AsTracking();
            }

            return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<Guid> AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Guid>> AddAsync(IEnumerable<Product> products)
        {
            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();
            return products.Select(x => x.Id).ToArray();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(IEnumerable<Product> products)
        {
            _dbContext.Products.UpdateRange(products);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(IEnumerable<Product> products)
        {
            _dbContext.Products.RemoveRange(products);
            await _dbContext.SaveChangesAsync();
        }
    }
}