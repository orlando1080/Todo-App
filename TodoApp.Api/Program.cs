using MassTransit;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Queues;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddOpenApiDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Servers.Clear();
        document.Servers.Add(new NSwag.OpenApiServer
        {
            Url = "http://localhost:5289"
        });
    };
});

// 1. Add services to the container (DI Container)
builder.Services.AddControllers();

// 2. Register the Database (Ledger)
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TaskCreatedConsumer>();

    x.UsingInMemory((ctx, cfg) => { cfg.ConfigureEndpoints(ctx); });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 3. Scrutor Auto Registration
// Group 1: Standard Abstractions (Interface -> Implementation)
// We want these hidden behind their interfaces.
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(CreateTaskCommandHandler), typeof(TodoRepository))
    .AddClasses(classes => classes.Where(c =>
        c.Name.EndsWith("Repository", StringComparison.Ordinal)
        || c.Name.EndsWith("Processor", StringComparison.Ordinal)
        || c.Name.EndsWith("UnitOfWork", StringComparison.Ordinal)
        || c.Name.EndsWith("MessageBus", StringComparison.Ordinal)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Group 2: Concrete Workers (Self-Registration)
// CommandHandlers usually don't need interfaces; we resolve the class directly.
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(CreateTaskCommandHandler))
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Handler", StringComparison.Ordinal)))
    .AsSelf()
    .WithScopedLifetime());

WebApplication app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseOpenApi();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();