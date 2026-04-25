using ToDoApp.Application.Tasks.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TodoAppDb _context;

    public UnitOfWork(TodoAppDb context)
    {
        _context = context;
    }


    public async Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
        await _context
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
}