using System;
using System.Collections.Generic;
using Integration.Service.Common.RabbitMq;

namespace France.Integration.Service
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("France Integration (Micro) Service Started");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
            RabbitMqReceiver client = new RabbitMqReceiver(exchange: "IntegrationR", queue: "France", routingKeys: new List<string>() { "Country.FRA" });
            client.ProcessMessages();
        }
    }
}
