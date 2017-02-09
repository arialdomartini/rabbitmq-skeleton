using RabbitMQ.Client;

namespace RabbitMQScheleton
{
    public interface IRabbitSetup
    {
        void Execute(IModel channel);
    }
}