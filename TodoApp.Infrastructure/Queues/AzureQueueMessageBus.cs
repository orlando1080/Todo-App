using System.Text.Json;
using Azure.Storage.Queues;
using TodoApp.Application.TodoTasks.Interfaces;

namespace TodoApp.Infrastructure.Queues;

public class AzureQueueMessageBus : IMessageBus
{
    private readonly string _connectionString;

    public AzureQueueMessageBus(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task PublishAsync<T>(T payload) where T : class
    {
        QueueClient client = new(_connectionString, payload?.GetType().Name.ToLowerInvariant(), new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        await client.CreateIfNotExistsAsync().ConfigureAwait(false);

        string message = JsonSerializer.Serialize(payload);

        // Base64 is required for Azure Queues

        await client.SendMessageAsync(message).ConfigureAwait(false);
    }
}