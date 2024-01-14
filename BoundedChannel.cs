using System.Collections.Concurrent;

namespace Channel;

public class BoundedChannel<T> : IChannel<T>
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly SemaphoreSlim _writeSemaphore;
    private readonly SemaphoreSlim _readSemaphore;

    public BoundedChannel(int capacity)
    {
        _writeSemaphore = new SemaphoreSlim(capacity, capacity);
        _readSemaphore = new SemaphoreSlim(0, capacity);
    }

    public async ValueTask WriteAsync(T item, CancellationToken cancellationToken = default)
    {
        await _writeSemaphore.WaitAsync(cancellationToken);
        _queue.Enqueue(item);
        _readSemaphore.Release();
    }

    public async ValueTask<T?> ReadAsync(CancellationToken cancellationToken = default)
    {
        await _readSemaphore.WaitAsync(cancellationToken);
        _queue.TryDequeue(out var item);
        _writeSemaphore.Release();
        return item;
    }
}