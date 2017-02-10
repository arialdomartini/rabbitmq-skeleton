using System;
using RabbitMQSkeleton;

namespace Runner
{
    public class BusinessLogic2 : IConsumerBusinessLogic
    {
        public void Handle<Payload2>(Payload2 message)
        {
            Console.WriteLine("## Invoked Business Logic  2");
        }
    }

    public class BusinessLogic3 : IConsumerBusinessLogic
    {
        public void Handle<Payload3>(Payload3 message)
        {
            Console.WriteLine("## Invoked Business Logic  3");
        }
    }
}