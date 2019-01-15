using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Service.Common.RabbitMq
{
    public class RabbitMqReceiver
    {
        private readonly string _exchange;
        private readonly ConnectionFactory _factory;
        private readonly string _queue;
        private readonly List<string> _keys;
        private IConnection _connection;

        /// <summary>
        /// Instance of message Receiver
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKeys"></param>
        public RabbitMqReceiver(string exchange, string queue, List<string> routingKeys)
        {
            _exchange = exchange;
            _queue = $"{exchange}_{queue}_Queue";
            _keys = routingKeys;
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        public void Close()
        {
            _connection.Close();
        }

        /// <summary>
        /// Process the incoming messages
        /// </summary>
        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(_exchange, "topic", true);
                    channel.QueueDeclare(_queue, true, false, false, null);

                    BindQueues(channel);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, args) =>
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(
                                    $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}: CorrelationId:{args.BasicProperties.CorrelationId} Message received:{Environment.NewLine}{Encoding.UTF8.GetString(args.Body)}");
                        Console.WriteLine("------------------------------------------------------------");                        
                    };

                    channel.BasicConsume(_queue, true, consumer);
                    Console.ReadLine();                   
                }
            }
        }

        /// <summary>
        /// Binding routing key to queue
        /// </summary>
        /// <param name="channel"></param>
        private void BindQueues(IModel channel)
        {
            if (_keys != null && _keys.Count > 0)
            {
                foreach (var key in _keys)
                {
                    channel.QueueBind(_queue, _exchange, $"{_exchange}.{key}");
                }
            }
        }
    }
}
