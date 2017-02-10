using System;
using RabbitMQ.Client;

namespace RabbitMQSkeleton
{
    public class NewSetupper<TPayload>
    {
        private readonly Func<IModel> _channelFactory;
        private readonly Func<IModel, IConsumerBusinessLogic<TPayload>, DefaultConsumer<TPayload>> _defaultConsumerFactory;
        private readonly Func<IModel, IConsumerBusinessLogic<TPayload>> _businessLogicFactory;

        public NewSetupper(
            Func<IModel> channelFactory, 
            Func<IModel, IConsumerBusinessLogic<TPayload>, DefaultConsumer<TPayload>> defaultConsumerFactory, 
            Func<IModel, IConsumerBusinessLogic<TPayload>> businessLogicFactory )
        {
            _channelFactory = channelFactory;
            _defaultConsumerFactory = defaultConsumerFactory;
            _businessLogicFactory = businessLogicFactory;
        }

        public void RegisterConsumerDomainLogic(IRabbitSetup rabbitSetup)
        {
            var channel = _channelFactory();
            var queue = Setup(rabbitSetup, channel);

            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");

            var consumerBusinessLogic = _businessLogicFactory(channel);
            var defaultConsumer = _defaultConsumerFactory(channel, consumerBusinessLogic);

            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }


        public string Setup(IRabbitSetup rabbitSetup, IModel resolve)
        {
            Console.WriteLine("## Setting up environment");

            return rabbitSetup.Execute(resolve);
        }
    }

    public class NewSetupper2<TPayload, TBusinessLogic>
    {
        private readonly Func<IModel> _channelFactory;
        private readonly Func<IModel, TBusinessLogic, DefaultConsumer<TPayload>> _defaultConsumerFactory;
        private readonly Func<IModel, TBusinessLogic> _businessLogicFactory;

        public NewSetupper2(
            Func<IModel> channelFactory, 
            Func<IModel, TBusinessLogic, DefaultConsumer<TPayload>> defaultConsumerFactory, 
            Func<IModel, TBusinessLogic> businessLogicFactory )
        {
            _channelFactory = channelFactory;
            _defaultConsumerFactory = defaultConsumerFactory;
            _businessLogicFactory = businessLogicFactory;
        }

        public void RegisterConsumerDomainLogic(IRabbitSetup rabbitSetup)
        {
            var channel = _channelFactory();
            var queue = Setup(rabbitSetup, channel);

            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");

            var consumerBusinessLogic = _businessLogicFactory(channel);
            var defaultConsumer = _defaultConsumerFactory(channel, consumerBusinessLogic);

            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }


        public string Setup(IRabbitSetup rabbitSetup, IModel resolve)
        {
            Console.WriteLine("## Setting up environment");

            return rabbitSetup.Execute(resolve);
        }
    }
}