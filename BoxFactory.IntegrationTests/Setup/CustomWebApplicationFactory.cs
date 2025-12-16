using System.Data;
using System.Data.Common;
using BoxFactoryAPI;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Testcontainers.PostgreSql;

namespace BoxFactory.IntegrationTests.Setup;

public class CustomWebApplicationFactory
    : WebApplicationFactory<IApiAssemblyMarker>, IAsyncLifetime
{
    private DbConnection _dbConnection = null!;
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
                .WithDatabase("boxfactorydb")
                .WithUsername("user")
                .WithPassword("password")
                .Build();
    
    public HttpClient Client { get; private set; } = null!;
    public ApplicationDbContext DbContext { get; set; } = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();
            
            var connectionString = _postgreSqlContainer.GetConnectionString();
        
            // Add IDbConnection to dependency injection
            services.AddScoped<IDbConnection>(_ =>
            {
                var connection = new NpgsqlConnection(connectionString ?? throw new Exception("Connection string cannot be null"));
                connection.Open();
                return connection;
            });
            
            services.AddDbContext<ApplicationDbContext>(x =>
                x.UseNpgsql(connectionString));
        });
        
        builder.UseEnvironment("Testing");
    }
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        Client = CreateClient();
        _dbConnection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString());
        await _dbConnection.OpenAsync();

        using var scope = Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await DbContext.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbConnection.CloseAsync();
        await _postgreSqlContainer.StopAsync();
    }
}