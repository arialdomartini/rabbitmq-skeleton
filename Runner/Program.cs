using System;
using Autofac;
using RabbitMQ.Client;
using RabbitMQSkeleton;

namespace Runner
{
    class Program
    {
        private readonly string _rabbitMQConnectionString;

        public static string Queue1 = "queue1";
        public static string Queue2 = "queue2";
        public static string Queue3 = "queue3";

        public Program()
        {
            _rabbitMQConnectionString = "amqp://192.168.99.100:5672";
        }

        static void Main(string[] args)
        {
            new Program().Run();
        }

        private void Run()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new RabbitMQSkeletonModule(_rabbitMQConnectionString));
            builder.RegisterModule(new RunnerModule());

            using (var container = builder.Build())
            {
                Console.WriteLine("## Requesting a ScheletonSetup");
                
                
                var businessLogic2 = container.Resolve<Func<BusinessLogic2>>();
//                RegisterConsumerDomainLogic(container, new SetupQueue2(), businessLogic2);
                RegisterConsumerDomainLogic<Payload2, BusinessLogic2>(container, new SetupQueue2());

    
                Console.WriteLine("## Waiting.... Press Enter to stop");
                Console.ReadLine();
            }
            Console.WriteLine("## Disposed. Press Enter to shutdown");
            Console.ReadLine();
        }


        public string Setup(IContainer container, IRabbitSetup rabbitSetup)
        {
            Console.WriteLine("## Setting up environment");
            var channel = container.Resolve<IModel>();

            return rabbitSetup.Execute(channel);
        }


        private void RegisterConsumerDomainLogic<TPayload, TBusinessLogic>(IContainer container, IRabbitSetup rabbitSetup) where TBusinessLogic : IConsumerBusinessLogic<TPayload>
        {
            var channel = container.Resolve<IModel>();
            var queue = Setup(container, rabbitSetup);

            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");

            var defaultConsumerFactory = container.Resolve<Func<IModel, IConsumerBusinessLogic<TPayload>, DefaultConsumer<TPayload>>>();

            var businessLogic = container.Resolve<TBusinessLogic>();
            var defaultConsumer = defaultConsumerFactory(channel, businessLogic);

            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }


        public void RegisterConsumerDomainLogic<T>(IContainer container, IRabbitSetup rabbitSetup, Func<IConsumerBusinessLogic<T>> businessLogic)
        {
            var channel = container.Resolve<IModel>();
            var queue = Setup(container, rabbitSetup);

            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");
            
            var defaultConsumerFactory = container.Resolve<Func<IModel, IConsumerBusinessLogic<T>, DefaultConsumer<T>>>();

            var defaultConsumer = defaultConsumerFactory(channel, businessLogic());
            
            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }


    }
}
