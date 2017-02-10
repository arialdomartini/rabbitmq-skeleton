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

                //                var scheletonSetup = container.Resolve<ScheletonSetup>();
                //
                //                scheletonSetup.Setup(new SetupQueue1());
                //
                //                var consumerFactory = container.Resolve<Func<IModel, MyConsumer>>();
                //                scheletonSetup.RegisterConsumer(consumerFactory, 5, Queue1);

                var scheletonSetup2 = container.Resolve<ScheletonSetup<Payload2>>();
                var businessLogic2 = container.Resolve<BusinessLogic2>();
                scheletonSetup2.RegisterConsumerDomainLogic(new SetupQueue2(), businessLogic2);


//                var scheletonSetup3 = container.Resolve<ScheletonSetup<Payload3>>();
//                var businessLogic3 = container.Resolve<BusinessLogic3>();
//                scheletonSetup3.RegisterConsumerDomainLogic(new SetupQueue3(), businessLogic3);


                var myExtendedConsumer = container.Resolve<MyExtendedConsumer>();
                myExtendedConsumer.Register();

                Console.WriteLine("## Waiting.... Press Enter to stop");
                Console.ReadLine();
            }
            Console.WriteLine("## Disposed. Press Enter to shutdown");
            Console.ReadLine();
        }
    }

    class MyExtendedConsumer : DefaultBaseConsumer<Payload3>
    {
        public MyExtendedConsumer(IModel channel) : base(channel)
        {
        }

        protected override void Handle(Payload3 payload)
        {
            Console.WriteLine("ricevuto");
        }

        protected override string Setup(IModel channel)
        {
            channel.QueueDeclare(Program.Queue3, durable: true, exclusive: true, autoDelete: false);
            channel.BasicConsume(Program.Queue3, noAck: true, consumer: this);
            return Program.Queue3;
        }
    }

}
