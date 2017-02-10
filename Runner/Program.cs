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
                RegisterConsumerDomainLogic(container, new SetupQueue2(), businessLogic2);


                RegisterConsumerDomainLogic<Payload2>(container, new SetupQueue2());

                RegisterConsumerDomainLogic<Payload2, BusinessLogic2>(container, new SetupQueue2());


                var newSetupper = container.Resolve<NewSetupper<Payload2>>();

                newSetupper.RegisterConsumerDomainLogic(new SetupQueue2());

                Console.WriteLine("## Waiting.... Press Enter to stop");
                Console.ReadLine();
            }
            Console.WriteLine("## Disposed. Press Enter to shutdown");
            Console.ReadLine();
        }


        public string Setup(IRabbitSetup rabbitSetup, IModel resolve)
        {
            Console.WriteLine("## Setting up environment");

            return rabbitSetup.Execute(resolve);
        }


        private void RegisterConsumerDomainLogic<TPayload, TBusinessLogic>(IContainer container, IRabbitSetup rabbitSetup) where TBusinessLogic : IConsumerBusinessLogic<TPayload>
        {
            var channel = container.Resolve<IModel>();
            var queue = Setup(rabbitSetup, channel);

            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");

            var defaultConsumerFactory = container.Resolve<Func<IModel, IConsumerBusinessLogic<TPayload>, DefaultConsumer<TPayload>>>();

            var businessLogic = container.Resolve<TBusinessLogic>();
            var defaultConsumer = defaultConsumerFactory(channel, businessLogic);

            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }


        public void RegisterConsumerDomainLogic<TPayload>(IContainer container, IRabbitSetup rabbitSetup, Func<IConsumerBusinessLogic<TPayload>> businessLogic)
        {
            var channel = container.Resolve<IModel>();
            var queue = Setup(rabbitSetup, channel);

            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");
            
            var defaultConsumerFactory = container.Resolve<Func<IModel, IConsumerBusinessLogic<TPayload>, DefaultConsumer<TPayload>>>();

            var defaultConsumer = defaultConsumerFactory(channel, businessLogic());
            
            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }

        public void RegisterConsumerDomainLogic<TPaylod>(IContainer container, IRabbitSetup rabbitSetup)
        {
            var channel = container.Resolve<IModel>();
            var queue = Setup(rabbitSetup, channel);

            Console.WriteLine($"## Creating a default consumer with the given domain logic and registering it");

            var defaultConsumerFactory = container.Resolve<Func<IModel, IConsumerBusinessLogic<TPaylod>, DefaultConsumer<TPaylod>>>();

            var businessLogic = container.Resolve<IConsumerBusinessLogic<TPaylod>>();
            var defaultConsumer = defaultConsumerFactory(channel, businessLogic);

            channel.BasicConsume(queue: queue, noAck: true, consumer: defaultConsumer);
        }
    }
}
