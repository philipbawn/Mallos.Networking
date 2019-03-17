namespace Mallos.Networking
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    public class ServicesCollection : IServiceProvider
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public void AddService<T>(T instance) => AddService(typeof(T), instance);
        public void AddService(Type type, object instance)
        {
            services[type] = instance;
        }

        public T GetService<T>() => (T)GetService(typeof(T));
        public object GetService(Type serviceType)
        {
            services.TryGetValue(serviceType, out var result);
            return result;
        }

        public void AddConfig(string[] args = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: false)
                .AddCommandLine(args ?? new string[0]);

            AddService<IConfiguration>(builder.Build());
        }
    }
}
