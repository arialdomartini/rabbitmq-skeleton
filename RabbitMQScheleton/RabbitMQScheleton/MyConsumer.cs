using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQScheleton
{
    public class MyConsumer : EventingBasicConsumer
    {
        private readonly IModel _channel;

        public MyConsumer(IModel channel) : base(channel)
        {
            _channel = channel;
        }
            
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);

            var payload = JsonConvert.DeserializeObject<Payload>(message);

            Console.WriteLine($"Received message foo:{payload.Foo}, bar:{payload.Bar}");
        }
    }
}