namespace Mallos.Networking
{
    using System;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Threading;

    class ServicesCollection : IServiceProvider
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public void AddService(Type type, object instance)
        {
            services[type] = instance;
        }

        public object GetService(Type serviceType)
        {
            return services[serviceType];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServicesCollection();
            serviceProvider.AddService(typeof(IConfiguration), new ConfigurationBuilder().Build());

            var server = new NetServer(serviceProvider);
            WaitUntilRunning(server);

            var client = new NetClient(serviceProvider, new NetConnectionParameters("", "", "127.0.0.1"));
            WaitUntilRunning(client);

            while (server.Running && client.Running)
            {
                client.SendMessage("Hello World");

                //server.NetPeer.SendMessage("ping");

                Thread.Sleep(100);
            }
        }

        static void WaitUntilRunning(NetPeer netPeer)
        {
            int retries = 100;
            while (retries > 0 && !netPeer.Running)
            {
                Thread.Sleep(10);
                retries -= 1;
            }
        }
    }
}
