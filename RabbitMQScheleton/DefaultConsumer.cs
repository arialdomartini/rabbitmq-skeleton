using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQSkeleton
{
    public class DefaultConsumer<T> : EventingBasicConsumer
    {
        private readonly IModel _channel;
        private readonly IConsumerBusinessLogic<T> _consumerBusinessLogic;

        public DefaultConsumer(IModel channel, IConsumerBusinessLogic<T> consumerBusinessLogic) : base(channel)
        {
            _channel = channel;
            _consumerBusinessLogic = consumerBusinessLogic;
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
                _consumerBusinessLogic.Handle(payload);
            }
            catch (Exception e)
            {
                Console.WriteLine("## Error: consumer business logic failed. Handle retry here.");
            }
            Console.WriteLine($"Received message foo:{message}");
        }
    }
}