// See https://aka.ms/new-console-template for more information

using Channel;

IChannel<string> channel = new BoundedChannel<string>(5);

var producer = Task.Run(async () =>
{
    for (var i = 0; i < 10; i++)
    {
        await channel.WriteAsync(i.ToString());
        Console.WriteLine($"Produced: {i}");
    }
});

var consumer = Task.Run(async () =>
{
    for (var i = 0; i < 10; i++)
    {
        var item = await channel.ReadAsync();
        Console.WriteLine($"Consumed: {item}");
    }
});

await Task.WhenAll(producer, consumer);