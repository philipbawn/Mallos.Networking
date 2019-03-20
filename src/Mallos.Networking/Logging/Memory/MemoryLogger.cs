namespace Mallos.Networking.Logging.Memory
{
    using Microsoft.Extensions.Logging;
    using System;

    public class MemoryLogger : ILogger
    {
        private readonly MemoryLoggerProvider provider;
        private readonly string category;

        public MemoryLogger(MemoryLoggerProvider loggerProvider, string categoryName)
        {
            this.provider = loggerProvider;
            this.category = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => provider.ScopeProvider?.Push(state);
        public bool IsEnabled(LogLevel logLevel) => provider.IsEnabled;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(DateTimeOffset.Now, logLevel, eventId, state, exception, formatter);
        }

        public void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            provider.AddMessage(new LogMessage(timestamp, logLevel, category, formatter(state, exception), exception));
        }
    }
}
