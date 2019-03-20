namespace Mallos.Networking.Logging.Memory
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public static class MemoryLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddMemory(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, MemoryLoggerProvider>();
            return builder;
        }

        public static ILoggingBuilder AddMemory(this ILoggingBuilder builder, Action<MemoryLoggerOptions> configure)
        {
            builder.AddMemory();
            builder.Services.Configure(configure ?? throw new ArgumentNullException(nameof(configure)));
            return builder;
        }
    }
}
