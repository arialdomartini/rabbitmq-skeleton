using System;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQScheleton;

namespace RabbitMQSkeleton
{
    public class SkeletonSetup
    {
        private readonly Func<IModel> _channelFactory;

        public SkeletonSetup(Func<IModel> channelFactory )
        {
            _channelFactory = channelFactory;
        }
        
        public void Setup(IRabbitSetup rabbitSetup)
        {
            Console.WriteLine("## Setting up environment");
            var channel = _channelFactory();

            rabbitSetup.Execute(channel);
        }

        public void RegisterConsumer(Func<IModel, EventingBasicConsumer> consumerFactory, int numberOfIntances, string queue)
        {
            Console.WriteLine("## Creating consumers and registering them");
            foreach (var i in Enumerable.Range(1, numberOfIntances))
            {
                Console.WriteLine($"## Creating consumer number {i} and registering it");
                var channel = _channelFactory();
                var consumer = consumerFactory(channel);
                channel.BasicConsume(queue: queue, noAck: true, consumer: consumer);
            }
        }

        public void RegisterConsumerDomainLogic<T>(IConsumerBusinessLogic<T> businessLogic, string queue, Func<Type, IModel, DefaultConsumer<T>  defaultConsumerFactory)
        {
            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");
            var channel = _channelFactory();
            var defaultConsumer = _defaultConsumerFactory<T>(typeof(T), channel);
            defaultConsumer.InjectBusinessLogic(businessLogic);
            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }
    }
}