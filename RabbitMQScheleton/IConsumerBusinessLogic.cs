namespace RabbitMQSkeleton
{
    public interface IConsumerBusinessLogic<T>
    {
        void Handle(T message);
    }
}