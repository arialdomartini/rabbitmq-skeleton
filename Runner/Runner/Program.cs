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
                Console.WriteLine("## Requesting a Starter");

                var starter = container.Resolve<Starter>();
                var configuration = new MyRabbitSetup();
                starter.Setup(configuration);

                starter.Start();


                Console.WriteLine("## Waiting.... Press Enter to stop");
                Console.ReadLine();
            }
            Console.WriteLine("## Disposed. Press Enter to shutdown");
            Console.ReadLine();
        }
    }

    internal class MyRabbitSetup : IRabbitSetup
    {
        public void Execute(IModel channel)
        {
            channel.QueueDeclare("myqueue", durable: true, exclusive: true, autoDelete: false);
        }
    }
}
