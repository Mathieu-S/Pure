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
    public class BrandRepository : IBrandRepository
    {
        private readonly PureDbContext _dbContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbContext">The DbContext.</param>
        /// <exception cref="ArgumentNullException">Throw when the dbContext is null.</exception>
        public BrandRepository(PureDbContext dbContext)
        {
            _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Brand>> GetAsync()
        {
            return await _dbContext.Brands.ToArrayAsync();
        }

        /// <inheritdoc/>
        public async Task<Brand> GetAsync(Guid id)
        {
            return await GetAsync(id, false);
        }

        /// <inheritdoc/>
        public async Task<Brand> GetAsync(Guid id, bool asTracking)
        {
            var query = _dbContext.Brands.AsQueryable();
        
            if (asTracking)
            {
                query = query.AsTracking();
            }

            return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<Guid> AddAsync(Brand brand)
        {
            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.SaveChangesAsync();
            return brand.Id;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Guid>> AddAsync(IEnumerable<Brand> brands)
        {
            await _dbContext.Brands.AddRangeAsync(brands);
            await _dbContext.SaveChangesAsync();
            return brands.Select(x => x.Id).ToArray();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Brand brand)
        {
            _dbContext.Brands.Update(brand);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(IEnumerable<Brand> brands)
        {
            _dbContext.Brands.UpdateRange(brands);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Brand brand)
        {
            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(IEnumerable<Brand> brands)
        {
            _dbContext.Brands.RemoveRange(brands);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Brand> GetByName(string name)
        {
            return await GetByName(name, false);
        }
        
        public async Task<Brand> GetByName(string name, bool asTracking)
        {
            var query = _dbContext.Brands.AsQueryable();
        
            if (asTracking)
            {
                query = query.AsTracking();
            }

            return await query.Where(x => x.Name == name).FirstOrDefaultAsync();
        }
    }
}