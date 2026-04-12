using MassTransit;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Services;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure;
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
        document.Servers.Add(new NSwag.OpenApiServer { Url = "http://localhost:5289" });
    };
});

// 1. Add services to the container (DI Container)
builder.Services.AddControllers();

// 2. Register the Database (Ledger)
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IMessageBus, MassTransitMessageBus>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CreateTaskCommandHandler>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TaskCreatedConsumer>();

    x.UsingInMemory((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 3. Scrutor Auto Registration
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(ITodoRepository), typeof(TodoRepository), typeof(ITodoApplicationService)) // Look at the assemblies these belong too
    .AddClasses(classes => classes.Where(c =>
        c.Name.EndsWith("Repository", StringComparison.Ordinal)
        || c.Name.EndsWith("ApplicationService", StringComparison.Ordinal))) // Filter the class by what it ends with
    .AsImplementedInterfaces() // Maps IToDoRepository -> ToDoRepository automatically
    .WithScopedLifetime()); // Sets the "Waitstaff" lifetime (new instance per request)

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