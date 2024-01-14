using System.Collections.Concurrent;

namespace Channel;

public class UnboundedChannel<T> : IChannel<T>
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly SemaphoreSlim _readSemaphore = new SemaphoreSlim(0);

    public ValueTask WriteAsync(T item, CancellationToken cancellationToken = default)
    {
        _queue.Enqueue(item);
        _readSemaphore.Release();
        return ValueTask.CompletedTask;
    }

    public async ValueTask<T?> ReadAsync(CancellationToken cancellationToken = default)
    {
        await _readSemaphore.WaitAsync(cancellationToken);
        _queue.TryDequeue(out var item);
        return item;
    }
}