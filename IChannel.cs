namespace Channel;

public interface IChannel<T>
{
    ValueTask WriteAsync(T item, CancellationToken cancellationToken = default);
    ValueTask<T> ReadAsync(CancellationToken cancellationToken = default);
}