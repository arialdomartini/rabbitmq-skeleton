using Autofac;
using RabbitMQSkeleton;

namespace Runner
{
    public class RunnerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MyConsumer>();

            builder.RegisterType<BusinessLogic2>().AsSelf();
            builder.RegisterType<BusinessLogic2>().As<IConsumerBusinessLogic<Payload2>>();
            builder.RegisterType<BusinessLogic3>();
            builder.RegisterType<MyExtendedConsumer>();

            builder.RegisterGeneric(typeof(NewSetupper<>));
        }
    }
}