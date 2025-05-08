namespace BookEvent.Core.Domain.Contracts.Persestence.DbInitializers
{
    public interface IBookEventDbInitializer
    {
        Task InitializeAsync();
        Task SeedAsync();
    }
}
