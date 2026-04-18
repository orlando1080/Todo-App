using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TodoDbContext _context;

    public UnitOfWork(TodoDbContext context)
    {
        _context = context;
    }


    public async Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
        await _context
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
}