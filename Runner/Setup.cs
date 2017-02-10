using RabbitMQ.Client;
using RabbitMQScheleton;
using RabbitMQSkeleton;

namespace Runner
{
    internal class SetupQueue1 : IRabbitSetup
    {
        public string Execute(IModel channel)
        {
            channel.QueueDeclare(Program.Queue1, durable: true, exclusive: true, autoDelete: false);
            return Program.Queue1;
        }
    }

    internal class SetupQueue2 : IRabbitSetup
    {
        public string Execute(IModel channel)
        {
            channel.QueueDeclare(Program.Queue2, durable: true, exclusive: true, autoDelete: false);
            return Program.Queue2;
        }
    }

    internal class SetupQueue3 : IRabbitSetup
    {
        public string Execute(IModel channel)
        {
            channel.QueueDeclare(Program.Queue3, durable: true, exclusive: true, autoDelete: false);
            return Program.Queue3;
        }
    }
}