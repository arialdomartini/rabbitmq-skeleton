using System;
using System.Linq;
using RabbitMQ.Client;

namespace RabbitMQScheleton
{
    public class ScheletonSetup
    {
        private readonly Func<IModel> _channelFactory;
        private readonly Func<IModel, MyConsumer> _consumerFactory;

        public ScheletonSetup(Func<IModel> channelFactory, Func<IModel, MyConsumer> consumerFactory)
        {
            _channelFactory = channelFactory;
            _consumerFactory = consumerFactory;
        }
        
        public void Setup(IRabbitSetup rabbitSetup)
        {
            Console.WriteLine("## Settign up environment");
            var channel = _channelFactory();

            rabbitSetup.Execute(channel);
        }

        public void RegisterConsumer(Func<IModel, MyConsumer> consumerFactory, int numberOfIntances)
        {
            Console.WriteLine("## Creating consumers and registering them");
            foreach (var i in Enumerable.Range(1, numberOfIntances))
            {
                Console.WriteLine($"## Creating consumer number {i} and registering it");
                var channel = _channelFactory();
                var consumer = _consumerFactory(channel);
                channel.BasicConsume(queue: "myqueue", noAck: true, consumer: consumer);
            }
        }
    }
}