using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pure.Application.Repositories
{
    /// <summary>
    /// A generic interface for CRUD on an entity.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IEntityRepository<T>
    {
        /// <summary>
        /// Get all entity type.
        /// </summary>
        /// <remarks>
        /// The entities are not tracked.
        /// </remarks>
        /// <returns>A list of entity.</returns>
        public Task<IEnumerable<T>> GetAsync();

        /// <summary>
        /// Get a specified entity.
        /// </summary>
        /// <remarks>
        /// The entity is not tracked.
        /// </remarks>
        /// <param name="id">The Guid of entity.</param>
        /// <returns>The specified entity.</returns>
        public Task<T> GetAsync(Guid id);

        /// <summary>
        /// Get a specified entity.
        /// </summary>
        /// <param name="id">The Guid of entity.</param>
        /// <param name="asTracking">Indicate whether the entity should be tracked.</param>
        /// <returns>The specified entity.</returns>
        public Task<T> GetAsync(Guid id, bool asTracking);

        /// <summary>
        /// Add an entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The Guid of created entity.</returns>
        public Task<Guid> AddAsync(T entity);

        /// <summary>
        /// Add a list of entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <returns>A list of Guid of created entity.</returns>
        public Task<IEnumerable<Guid>> AddAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public Task UpdateAsync(T entity);

        /// <summary>
        /// Update a list of entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public Task DeleteAsync(T entity);

        /// <summary>
        /// Delete a list of entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        public Task DeleteAsync(IEnumerable<T> entities);
    }
}