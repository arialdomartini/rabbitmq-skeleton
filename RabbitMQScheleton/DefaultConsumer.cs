using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQSkeleton
{
    public class DefaultConsumer : EventingBasicConsumer
    {
        private readonly Type t;
        private readonly IModel _channel;
        private readonly IConsumerBusinessLogic _consumerBusinessLogic;

        public DefaultConsumer(Type t, IModel channel, IConsumerBusinessLogic consumerBusinessLogic) : base(channel)
        {
            this.t = t;
            _channel = channel;
            _consumerBusinessLogic = consumerBusinessLogic;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            object payload = null;
            string message = null;
            try
            {
                message = Encoding.UTF8.GetString(body);
                payload = JsonConvert.DeserializeObject(message, t);
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