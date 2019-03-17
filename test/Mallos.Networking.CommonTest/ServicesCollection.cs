namespace Mallos.Networking
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    public class ServicesCollection : IServiceProvider
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
}
