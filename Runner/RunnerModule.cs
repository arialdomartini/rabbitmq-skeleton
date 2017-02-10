using Autofac;

namespace Runner
{
    public class RunnerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MyConsumer>();

            builder.RegisterType<BusinessLogic2>();
            builder.RegisterType<BusinessLogic3>();
        }
    }
}