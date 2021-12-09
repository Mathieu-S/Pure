using Xunit;

namespace Pure.Tests.Unit.TestHelpers
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}