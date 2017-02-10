using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQScheleton
{
    public class DefaultConsumer : EventingBasicConsumer
    {
        private readonly IModel _channel;
        private readonly IConsumerBusinessLogic _consumerBusinessLogic;

        public DefaultConsumer(IModel channel, IConsumerBusinessLogic consumerBusinessLogic) : base(channel)
        {
            _channel = channel;
            _consumerBusinessLogic = consumerBusinessLogic;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            Payload payload = null;
            try
            {
                var message = Encoding.UTF8.GetString(body);
                payload = JsonConvert.DeserializeObject<Payload>(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("## Error: can't deserialise body message. Handle NACK here.");
                Console.WriteLine(e);
            }

            try
            {
                _consumerBusinessLogic.Handle();
            }
            catch (Exception e)
            {
                Console.WriteLine("## Error: consumer business logic failed. Handle retry here.");
            }
            Console.WriteLine($"Received message foo:{payload.Foo}, bar:{payload.Bar}");
        }
    }
}