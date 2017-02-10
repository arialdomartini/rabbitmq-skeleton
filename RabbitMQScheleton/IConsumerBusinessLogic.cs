namespace RabbitMQSkeleton
{
    public interface IConsumerBusinessLogic
    {
        void Handle<T>(T message);
    }
}