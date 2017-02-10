using System;
using System.Linq;
using RabbitMQ.Client;

namespace RabbitMQScheleton
{
    public class ScheletonSetup
    {
        private readonly Func<IModel> _channelFactory;
        private readonly Func<IConsumerBusinessLogic, IModel, DefaultConsumer> _defaultConsumerFactory;

        public ScheletonSetup(Func<IModel> channelFactory, Func<IConsumerBusinessLogic, IModel, DefaultConsumer>  defaultConsumerFactory )
        {
            _channelFactory = channelFactory;
            _defaultConsumerFactory = defaultConsumerFactory;
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
                var consumer = consumerFactory(channel);
                channel.BasicConsume(queue: "myqueue", noAck: true, consumer: consumer);
            }
        }

        public void RegisterConsumerDomainlogic(IConsumerBusinessLogic businessLogic, string queue)
        {
            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");
            var channel = _channelFactory();
            var consumer = _defaultConsumerFactory(businessLogic, channel);
            channel.BasicConsume(queue: queue, noAck: true, consumer: consumer);
        }
    }
}