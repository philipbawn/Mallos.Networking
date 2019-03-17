namespace Mallos.Networking
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    public static class Services
    {
        public static IServiceProvider Create(string[] args = null)
        {
            var serviceCollection = new ServiceCollection();

            var config = BuildConfig(args);
            serviceCollection.AddSingleton(config);

            serviceCollection.AddLogging((logBuilder) =>
            {
                logBuilder.AddConfiguration(config.GetSection("Logging"));
                logBuilder.AddConsole();
            });

            return serviceCollection.BuildServiceProvider();
        }

        private static IConfiguration BuildConfig(string[] args = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: false)
                .AddCommandLine(args ?? new string[0]);

            return builder.Build();
        }

        public static T GetService<T>(this IServiceProvider serviceProvider)
            => (T)serviceProvider.GetService(typeof(T));
    }
}
