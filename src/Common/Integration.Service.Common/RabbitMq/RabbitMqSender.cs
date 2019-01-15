using RabbitMQ.Client;
using System.Text;

namespace Integration.Service.Common.RabbitMq
{
    public class RabbitMqSender
    {
        private readonly IConnection _connection;
        private readonly IConnectionFactory _factory;
        private readonly string _exchange;
        //private readonly IModel _model;
        private readonly string _routingKey;

        /// <summary>
        /// Instance of message sender
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="topic"></param>
        public RabbitMqSender(string exchange, string topic)
        {
            _factory = new ConnectionFactory { HostName = "localhost" };
            _exchange = exchange;
            _routingKey = $"{exchange}.{topic}";          
        }      

        public void Publish(string message, string correlationId)
        {

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(_exchange, "topic", true);

                var props = channel.CreateBasicProperties();
                //props.ContentType = "text/xml"; // specify type explicitly if needed
                //props.Headers = new Dictionary<string, object> { { "country", 250 } }; // add optional headers            
                props.DeliveryMode = 2;
                props.CorrelationId = correlationId;
                channel.BasicPublish(_exchange, _routingKey, props, Encoding.UTF8.GetBytes(message));
            }
        }      
    }
}
