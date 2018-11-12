using System;
using System.Collections.Generic;
using Integration.Service.Common.RabbitMq;

namespace Integration.Service.Broker
{
    public class IntegrationBroker
    {
        /// <summary>
        /// Send message to Rabbitmq
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="message"></param>
        public void SendMessage(string exchange, string message)
        {
            var topics = GetSubscriptionTopics(message);
            var correlationId = Guid.NewGuid().ToString();

            foreach (var topic in topics)
            {
                using (var rabbit = new RabbitMqSender(exchange: exchange, topic: topic))
                {
                    rabbit.SendMessage(message, correlationId);
                }
            }
            Console.WriteLine(
                $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}: Message with CorrelationId: {correlationId} was sent to the integration broker: {message}");
            Console.WriteLine("------------------------------------------------------------");
        }

        /// <summary>
        /// Get Subscriber Routing Keys/ Topics
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string[] GetSubscriptionTopics(string message)
        {
            List<string> topics = new List<string>();

            // Note:
            // Content Based Routing - Not a recommended approach. The alternative approaches are to let publisher or subscriber handle this.                
            // List few topics and describe what it does, let the customers choose or request a new one.
            // If no need of content based routing topics will be retrieved from a subscription store.
            // Exchange needs to be known and topic will be tied to exchange.

            // All Messages
            if (message.Trim().StartsWith("<Data>") && message.Trim().EndsWith("</Data>"))
            {
                // Australia
                if (message.Contains("<Country>AUS</Country>"))
                    topics.Add("Country.AUS");

                // France
                else if (message.Contains("<Country>FRA</Country>"))
                    topics.Add("Country.FRA");

                // All other countries
                else topics.Add("Country.*");
            }

            // If needed can set an invalid queue to handle invalid messages.
            if (topics.Count.Equals(0))
                topics.Add("Invalid");

            return topics.ToArray();
        }
    }
}
