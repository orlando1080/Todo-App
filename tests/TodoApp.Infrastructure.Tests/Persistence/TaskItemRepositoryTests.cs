#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Testcontainers.MsSql;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Infrastructure.Tests.Persistence;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1001:Types that own disposable fields should be disposable",
    Justification = "Disposed in TearDown")]

[TestFixture]
[TestOf(typeof(TaskItemRepository))]
internal sealed class TaskItemRepositoryTests
{
    private TaskItem _taskItem = null!;

    private MsSqlContainer _sqlContainer = null!;

    private TodoAppDb _context = null!;

    private TaskItemRepository _sut = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _sqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();
        await _sqlContainer.StartAsync().ConfigureAwait(false);

        DbContextOptions<TodoAppDb> options = new DbContextOptionsBuilder<TodoAppDb>()
            .UseSqlServer(_sqlContainer.GetConnectionString())
            .Options;

        await using TodoAppDb migrationContext = new(options);

        await migrationContext.Database.MigrateAsync().ConfigureAwait(false);
    }

    [SetUp]
    public async Task SetUp()
    {
        DbContextOptions<TodoAppDb> options = new DbContextOptionsBuilder<TodoAppDb>()
            .UseSqlServer(_sqlContainer.GetConnectionString())
            .Options;

        _context = new TodoAppDb(options);
        _taskItem = TaskItem.Create("test");
        _context.TaskItems.Add(_taskItem);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        _sut = new TaskItemRepository(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.TaskItems.ExecuteDeleteAsync().ConfigureAwait(false);
        await _context.DisposeAsync().ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _sqlContainer.DisposeAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task DeleteAsync_ValidId_DeletesTaskItem()
    {
        bool result = await _sut.DeleteAsync(_taskItem.Id).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(await _context.TaskItems.AnyAsync(t => t.Id == _taskItem.Id).ConfigureAwait(false), Is.False);
        }
    }

    [Test]
    public async Task DeleteAsync_InvalidId_ReturnsFalse()
    {
        bool result = await _sut.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.False);
            Assert.That(await _context.TaskItems.AnyAsync(t => t.Id == _taskItem.Id).ConfigureAwait(false), Is.True);
        }
    }

    [Test]
    public async Task Add_ValidTaskItem_AddsTaskItemToContext()
    {
        TaskItem taskItem = TaskItem.Create("test");

        await _sut.Add(taskItem, CancellationToken.None);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        Assert.That(await _context.TaskItems.AnyAsync(t => t.Id == taskItem.Id).ConfigureAwait(false), Is.True);
    }

    [Test]
    public async Task Update_ValidTaskItem_UpdatesTaskItemInContext()
    {
        _taskItem.ToggleIsCompleted();

        await _sut.Update(_taskItem);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        TaskItem? persistedTaskItem = await _context.TaskItems.AsNoTracking()
            .SingleOrDefaultAsync(t => t.Id == _taskItem.Id).ConfigureAwait(false);

        Assert.That(persistedTaskItem?.IsCompleted, Is.True);
    }

    [Test]
    public async Task GetByIdAsync_ValidId_ReturnsTaskItem()
    {
        TaskItem? result = await _sut.GetByIdAsync(_taskItem.Id).ConfigureAwait(false);

        Assert.That(result?.Id, Is.EqualTo(_taskItem.Id));
    }

    [Test]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        TaskItem? result = await _sut.GetByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_HydrateTaskItems_ReturnsTaskItems()
    {
        TaskItem[] result = await _sut.GetAllAsync().ConfigureAwait(false);

        Assert.That(result.Length, Is.EqualTo(1));
    }

    [Test]
    public async Task GetAllAsync_NoTaskItems_ReturnsEmptyArray()
    {
        await _context.TaskItems.ExecuteDeleteAsync().ConfigureAwait(false);

        TaskItem[] result = await _sut.GetAllAsync().ConfigureAwait(false);

        Assert.That(result, Is.Empty);
    }
}