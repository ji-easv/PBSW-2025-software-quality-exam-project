using System.Text.Json.Serialization;
using BoxFactoryAPI;
using BoxFactoryAPI.Exceptions;
using Core.Mapping;
using Core.Services;
using Core.Services.Interfaces;
using Infrastructure;
using Infrastructure.Implementations;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BoxFactoryDatabase") ??
                           throw new InvalidOperationException(
                               "Connection string 'BoxFactoryDatabase' not found.");
    options.UseNpgsql(connectionString);
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<IBoxRepository, BoxRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IStatsRepository, StatsRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<DbInitialize>();

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    if (args.Contains("db-init") || args.Contains("--db-init"))
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitialize>();
        await dbInitializer.InitializeData();
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();