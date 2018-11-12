using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

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
            _queue = queue;
            _keys = routingKeys;
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
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
                    channel.QueueDeclare($"{_exchange}_{_queue}_Queue", true, false, false, null);

                    BindQueues(channel);

                    channel.BasicQos(0, 1, false);

                    Subscription subscription = new Subscription(channel, $"{_exchange}_{_queue}_Queue", false);

                    while (true)
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();
                        var message = deliveryArguments.Body;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(
                            $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}: CorrelationId:{deliveryArguments.BasicProperties.CorrelationId} Message received:{Environment.NewLine}{Encoding.UTF8.GetString(message)}");
                        Console.WriteLine("------------------------------------------------------------");
                        subscription.Ack(deliveryArguments);
                    }
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
                    channel.QueueBind($"{_exchange}_{_queue}_Queue", _exchange, $"{_exchange}.{key}");
                }
            }
        }
    }
}
