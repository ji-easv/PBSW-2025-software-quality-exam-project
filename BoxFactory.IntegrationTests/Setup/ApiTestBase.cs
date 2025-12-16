using Infrastructure;

namespace BoxFactory.IntegrationTests.Setup;

public class ApiTestBase : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private bool _disposed;
    protected readonly HttpClient Client;

    protected ApplicationDbContext NewDbContext
    {
        get
        {
            var scope = _factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }

    protected ApiTestBase(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        Client = factory.Client;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            // Dispose managed resources here if needed
            // Currently no managed resources to dispose
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}