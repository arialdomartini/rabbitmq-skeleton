using Autofac;

namespace Runner
{
    public class RunnerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MyConsumerBusinessLogic>();
        }
    }
}