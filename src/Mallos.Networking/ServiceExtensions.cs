namespace Mallos.Networking
{
    using Microsoft.Extensions.Logging;
    using System;

    static class ServiceExtensions
    {
        public static ILogger TryCreateLogger<T>(this IServiceProvider serviceProvider)
        {
            var loggerFactory = (ILoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
            if (loggerFactory != null)
            {
                return loggerFactory.CreateLogger<T>();
            }
            return null;
        }
    }
}
