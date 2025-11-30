using BandLab.Scaling.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace BandLab.Scaling.Queue;

internal abstract class ScaleQueue(ScalingQueueOptions options, ILogger log) : IHostedService, IAsyncDisposable
{
    protected const string tasks = "ScaleTasks";
    protected const string done = "ScaleDone";

    private readonly BasicProperties properties = new();
    private readonly ConnectionFactory factory = new()
    {
        Uri = new Uri(options.Connection
            ?? throw new InvalidOperationException("Connection to queue is not defined"))
    };

    protected IConnection? connection;
    protected IChannel? channel;

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        if (connection is not null)
            return;

        try
        {
            connection = await factory.CreateConnectionAsync(cancellationToken);
            channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(tasks, durable: false, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(done, durable: false, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            await Connected();
        }
        catch (TaskCanceledException) { }
        catch (Exception e)
        {
            log.LogError(e, "Failed to connect to Rabbit: {scaling.queue.failure}", e.Message);
        }
    }

    protected virtual Task Connected() =>
        Task.CompletedTask;

    protected ValueTask Publish(string queue, Scale value)
    {
        if (channel is null)
            throw new InvalidCastException("Channel is not connected");

        return channel.BasicPublishAsync("", queue, true, properties,
            JsonSerializer.SerializeToUtf8Bytes(value, ScaleModelsSerialization.Default.Scale),
            CancellationToken.None);
    }

    protected async Task Subscribe(string queue, Func<Scale, Task> action)
    {
        if (channel is null)
            throw new InvalidCastException("Channel is not connected");

        var listen = new AsyncEventingBasicConsumer(channel);
        listen.ReceivedAsync += async (origin, ea) =>
        {
            try
            {
                var data = JsonSerializer.Deserialize(ea.Body.Span, ScaleModelsSerialization.Default.Scale)
                    ?? throw new InvalidOperationException($"Failed to deserialize data to {typeof(Scale)}");

                await action(data);

                await channel!.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                log.LogError(e, "Failed to process event: {failure}", e.Message);
            }
        };

        await channel.BasicConsumeAsync(queue, autoAck: false, "", true, false, null, listen);
    }

    async Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        if (channel is not null)
            await channel.CloseAsync(cancellationToken: cancellationToken);

        if (connection is not null)
            await connection.CloseAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await (channel?.DisposeAsync() ?? default);
        await (connection?.DisposeAsync() ?? default);
    }
}
