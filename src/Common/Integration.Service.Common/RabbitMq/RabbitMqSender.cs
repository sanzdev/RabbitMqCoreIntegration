using System;
using System.Text;
using RabbitMQ.Client;

namespace Integration.Service.Common.RabbitMq
{
    public class RabbitMqSender: IDisposable
    {
        private readonly IConnection _connection;
        private readonly string _exchange;
        private readonly IModel _model;
        private readonly string _routingKey;

        /// <summary>
        /// Instance of message sender
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="topic"></param>
        public RabbitMqSender(string exchange, string topic)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _exchange = exchange;
            _routingKey = $"{exchange}.{topic}";

            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(exchange, "topic", true);

            // Queue can be declared by the Publisher as well as the subscriber as the creation is idempotent.
            // Refer Best Practices.

            // Optional for Custom Errors to handle invalid messages
            if (topic == "Invalid")
            {
                CreateInvalidQueue(exchange, topic);
            }
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            _connection.Close();
        }

        public void SendMessage(string message, string correlationId)
        {
            var props = _model.CreateBasicProperties();                        
            //props.ContentType = "text/xml"; // specify type explicitly if needed
            //props.Headers = new Dictionary<string, object> { { "country", 250 } }; // add optional headers            
            props.DeliveryMode = 2;
            props.CorrelationId = correlationId;            
            _model.BasicPublish(_exchange, _routingKey, props, Encoding.UTF8.GetBytes(message));
        }

        private void CreateInvalidQueue(string exchange, string topic)
        {
            if (!string.IsNullOrEmpty(topic))
            {
                _model.QueueDeclare($"{exchange}_{topic}_Queue", true, false, false, null);
                _model.QueueBind($"{exchange}_{topic}_Queue", exchange, _routingKey);
            }
        }
    }
}
