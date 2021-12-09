using System;
using Microsoft.EntityFrameworkCore;
using Pure.Domain.Models;
using Pure.Persistence;

namespace Pure.Tests.Unit.TestHelpers
{
    public class DatabaseFixture : IDisposable
    {
        public PureDbContext PureDbContext { get; }
        
        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<PureDbContext>()
                .UseInMemoryDatabase("Test")
                .Options;

            PureDbContext = new PureDbContext(options);

            SeedContext();
        }
        
        private void SeedContext()
        {
            var acme = new Brand
            {
                Id = new Guid("2c9f6032-9d43-4fa2-ac64-9c5750f0c504"),
                Name = "Acme"
            };
            var cola = new Product
            {
                Id = new Guid("fa023462-8ecf-4d63-97d6-ebece647299f"),
                Name = "Cola",
                Description = "A soda",
                Brand = acme,
                Price = 1
            };
            var pizza = new Product
            {
                Id = new Guid("e0fe5fa6-374d-4eca-8534-5fc3e3d01f7c"),
                Name = "Pizza",
                Description = "A neapolitan pizza",
                Brand = acme,
                Price = 5
            };

            PureDbContext.Add(acme);
            PureDbContext.Add(cola);
            PureDbContext.Add(pizza);
            PureDbContext.SaveChanges();
        }

        public void Dispose()
        {
            PureDbContext.Dispose();
        }
    }
}