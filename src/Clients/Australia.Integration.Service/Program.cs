using System;
using System.Collections.Generic;
using Integration.Service.Common.RabbitMq;

namespace Australia.Integration.Service
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Australia Integration (Micro) Service Started");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
            RabbitMqReceiver client = new RabbitMqReceiver(exchange: "IntegrationR", queue: "Australia", routingKeys: new List<string>() { "Country.AUS" });
            client.ProcessMessages();
        }
    }
}
