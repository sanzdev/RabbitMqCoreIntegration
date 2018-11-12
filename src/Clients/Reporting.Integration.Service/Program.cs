using System;
using System.Collections.Generic;
using Integration.Service.Common.RabbitMq;

namespace Reporting.Integration.Service
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Reporting Integration (Micro) Service Started");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine();
            RabbitMqReceiver client = new RabbitMqReceiver(exchange: "IntegrationR", queue: "Reporting", routingKeys: new List<string>() { "Country.*" });
            client.ProcessMessages();
        }
    }
}
