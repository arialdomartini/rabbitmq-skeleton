using System;
using RabbitMQSkeleton;

namespace Runner
{
    public class BusinessLogic2 : IConsumerBusinessLogic<Payload2>
    {
        public void Handle(Payload2 message)
        {
            Console.WriteLine("## Invoked Business Logic  2");
        }
    }

    public class BusinessLogic3 : IConsumerBusinessLogic<Payload3>
    {
        public void Handle(Payload3 message)
        {
            Console.WriteLine("## Invoked Business Logic  3");
        }
    }
}