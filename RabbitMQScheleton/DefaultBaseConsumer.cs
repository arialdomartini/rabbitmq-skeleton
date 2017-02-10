using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQSkeleton
{
    public abstract class DefaultBaseConsumer<T> : EventingBasicConsumer
    {
        private readonly IModel _channel;

        protected DefaultBaseConsumer(IModel channel) : base(channel)
        {
            _channel = channel;
        }

        public void Register()
        {
            var queue = Setup(_channel);
//            channel.BasicConsume(queue: queue, noAck: true, consumer: this);
        }


        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            var payload = default(T);
            string message = null;
            try
            {
                message = Encoding.UTF8.GetString(body);
                payload = JsonConvert.DeserializeObject<T>(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("## Error: can't deserialise body message. Handle NACK here.");
                Console.WriteLine(e);
            }

            try
            {
                Handle(payload);
            }
            catch (Exception e)
            {
                Console.WriteLine("## Error: consumer business logic failed. Handle retry here.");
            }
            Console.WriteLine($"Received message foo:{message}");
        }

        protected abstract void Handle(T payload);

        protected abstract string Setup(IModel channel);
    }
}