using RabbitMQ.Client;

namespace RabbitMQSkeleton
{
    public interface IRabbitSetup
    {
        string Execute(IModel channel);
    }
}