using System;
using RabbitMQ.Client;
using RabbitMQSkeleton;

namespace Runner
{
    class MyExtendedConsumer : DefaultBaseConsumer<Payload3>
    {
        public MyExtendedConsumer(IModel channel) : base(channel)
        {
        }

        protected override void Handle(Payload3 payload)
        {
            Console.WriteLine("ricevuto");
        }

        protected override string Setup(IModel channel)
        {
            channel.QueueDeclare(Program.Queue3, durable: true, exclusive: true, autoDelete: false);
            channel.BasicConsume(Program.Queue3, noAck: true, consumer: this);
            return Program.Queue3;
        }
    }
}