using RabbitMQ.Client;

namespace RabbitMQScheleton
{
    public interface IRabbitSetup
    {
        string Execute(IModel channel);
    }
}