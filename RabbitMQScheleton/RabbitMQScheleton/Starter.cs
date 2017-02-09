using System;
using RabbitMQ.Client;

namespace RabbitMQScheleton
{
    public class Starter
    {
        private readonly Func<IModel> _channelFactory;
        private readonly Func<IModel, MyConsumer> _consumerFactory;

        public Starter(Func<IModel> channelFactory, Func<IModel, MyConsumer> consumerFactory)
        {
            _channelFactory = channelFactory;
            _consumerFactory = consumerFactory;
        }

        public void Start()
        {
            Console.WriteLine("## Creating consumers and registering them");
            var channel = _channelFactory();
            var consumer = _consumerFactory(channel);
            channel.BasicConsume(queue: "myqueue", noAck: true, consumer: consumer);
        }

        public void Setup(IRabbitSetup rabbitSetup)
        {
            Console.WriteLine("## Settign up environment");
            var channel = _channelFactory();

            rabbitSetup.Execute(channel);
        }
    }
}