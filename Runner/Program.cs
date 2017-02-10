using System;
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

            using (var container = builder.Build())
            {
                Console.WriteLine("## Requesting a ScheletonSetup");

                var scheletonSetup = container.Resolve<ScheletonSetup>();
                scheletonSetup.Setup(new Setup1());
                scheletonSetup.Setup(new Setup2());

                var consumerFactory = container.Resolve<Func<IModel, MyConsumer>>();
                scheletonSetup.RegisterConsumer(consumerFactory, 5);

                Console.WriteLine("## Waiting.... Press Enter to stop");
                Console.ReadLine();
            }
            Console.WriteLine("## Disposed. Press Enter to shutdown");
            Console.ReadLine();
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
            channel.QueueDeclare("foo", durable: true, exclusive: true, autoDelete: false);
            channel.QueueDeclare("", durable: true, exclusive: true, autoDelete: false);
        }
    }
}
