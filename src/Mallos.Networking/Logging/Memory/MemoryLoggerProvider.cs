namespace Mallos.Networking.Logging.Memory
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MemoryLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        public bool IsEnabled => options.IsEnabled;

        public IReadOnlyList<LogMessage> Messages => messages;

        public IExternalScopeProvider ScopeProvider => options.IncludeScopes ? scopeProvider : null;

        private readonly List<LogMessage> messages = new List<LogMessage>();
        private readonly IDisposable optionsChangeToken;

        private MemoryLoggerOptions options;
        private IExternalScopeProvider scopeProvider;

        public MemoryLoggerProvider(IOptionsMonitor<MemoryLoggerOptions> options)
        {
            this.options = options.CurrentValue;
            this.optionsChangeToken = options.OnChange(value => this.options = value);
        }

        protected virtual void OnRemoveMessages(IEnumerable<LogMessage> messages)
        {

        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MemoryLogger(this, categoryName);
        }

        public void Dispose()
        {
            optionsChangeToken?.Dispose();
        }

        internal void AddMessage(LogMessage message)
        {
            if (messages.Count > options.MaxMessages)
            {
                var oldMessages = messages.Take(options.BulkRemoveCount);
                OnRemoveMessages(oldMessages);
                messages.RemoveRange(0, options.BulkRemoveCount);
            }

            messages.Add(message);
        }

        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider) => this.scopeProvider = scopeProvider;
    }
}
