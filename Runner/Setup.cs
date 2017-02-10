using RabbitMQ.Client;
using RabbitMQScheleton;
using RabbitMQSkeleton;

namespace Runner
{
    internal class SetupQueue1 : IRabbitSetup
    {
        public void Execute(IModel channel)
        {
            channel.QueueDeclare(Program.Queue1, durable: true, exclusive: true, autoDelete: false);
        }
    }

    internal class SetupQueue2 : IRabbitSetup
    {
        public void Execute(IModel channel)
        {
            channel.QueueDeclare(Program.Queue2, durable: true, exclusive: true, autoDelete: false);
        }
    }

    internal class SetupQueue3 : IRabbitSetup
    {
        public void Execute(IModel channel)
        {
            channel.QueueDeclare(Program.Queue3, durable: true, exclusive: true, autoDelete: false);
        }
    }
}