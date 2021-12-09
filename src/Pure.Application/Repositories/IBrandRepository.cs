using System.Threading.Tasks;
using Pure.Domain.Models;

namespace Pure.Application.Repositories
{
    /// <summary>
    /// The repository of <see cref="Brand"/> entity.
    /// </summary>
    public interface IBrandRepository : IEntityRepository<Brand>
    {
        /// <summary>
        /// Find a Brand by its name.
        /// </summary>
        /// <param name="name">The name of brand.</param>
        /// <returns></returns>
        public Task<Brand> GetByName(string name);
        public Task<Brand> GetByName(string name, bool asTracking);
    }
}