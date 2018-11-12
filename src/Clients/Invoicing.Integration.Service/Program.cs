using System;
using System.Collections.Generic;
using Integration.Service.Common.RabbitMq;

namespace Invoicing.Integration.Service
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Invoicing Integration (Micro) Service Started");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
            RabbitMqReceiver client = new RabbitMqReceiver(exchange: "IntegrationR", queue: "Invoicing", routingKeys: new List<string>() { "Country.AUS", "Country.FRA" });
            client.ProcessMessages();
        }
    }
}
