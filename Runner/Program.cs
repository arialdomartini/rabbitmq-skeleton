using System;
using System.Diagnostics;
using Autofac;
using RabbitMQ.Client;
using RabbitMQScheleton;

namespace Runner
{
    class Program
    {
        private readonly string _rabbitMQConnectionString;

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

            builder.RegisterModule(new RabbitMQScheletonModule(_rabbitMQConnectionString));
            builder.RegisterModule(new RunnerModule());

            using (var container = builder.Build())
            {
                Console.WriteLine("## Requesting a ScheletonSetup");

                var scheletonSetup = container.Resolve<ScheletonSetup>();
                scheletonSetup.Setup(new Setup1());

                var consumerFactory = container.Resolve<Func<IModel, MyConsumer>>();
                scheletonSetup.RegisterConsumer(consumerFactory, 5);

                scheletonSetup.Setup(new Setup2());
                var businessLogic = container.Resolve<MyConsumerBusinessLogic>();
                scheletonSetup.RegisterConsumerDomainlogic(businessLogic, "another_queue");


                Console.WriteLine("## Waiting.... Press Enter to stop");
                Console.ReadLine();
            }
            Console.WriteLine("## Disposed. Press Enter to shutdown");
            Console.ReadLine();
        }
    }

    internal class MyConsumerBusinessLogic : IConsumerBusinessLogic
    {
        public void Handle()
        {
            Console.WriteLine("## Consumer domain logic invoked");
        }
    }

    internal class Setup1 : IRabbitSetup
    {
        public void Execute(IModel channel)
        {
            channel.QueueDeclare("myqueue", durable: true, exclusive: true, autoDelete: false);
        }
    }


    internal class Setup2 : IRabbitSetup
    {
        public void Execute(IModel channel)
        {
            channel.QueueDeclare("another_queue", durable: true, exclusive: true, autoDelete: false);
        }
    }
}
