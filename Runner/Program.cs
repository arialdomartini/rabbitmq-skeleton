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

                var scheletonSetup = container.Resolve<ScheletonSetup>();

                scheletonSetup.Setup(new SetupQueue1());

                var consumerFactory = container.Resolve<Func<IModel, MyConsumer>>();
                scheletonSetup.RegisterConsumer(consumerFactory, 5, Queue1);

                scheletonSetup.Setup(new SetupQueue2());
                var businessLogic2 = container.Resolve<BusinessLogic2>();
                scheletonSetup.RegisterConsumerDomainLogic(businessLogic2, Queue2);


                scheletonSetup.Setup(new SetupQueue3());
                var businessLogic3 = container.Resolve<BusinessLogic3>();
                scheletonSetup.RegisterConsumerDomainLogic(businessLogic3, Queue3);


                Console.WriteLine("## Waiting.... Press Enter to stop");
                Console.ReadLine();
            }
            Console.WriteLine("## Disposed. Press Enter to shutdown");
            Console.ReadLine();
        }
    }
}
