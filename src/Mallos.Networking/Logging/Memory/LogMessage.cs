namespace Mallos.Networking.Logging.Memory
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;

    public struct LogMessage
    {
        public readonly DateTimeOffset Timestamp;
        public readonly LogLevel LogLevel;
        public readonly string Message;
        public readonly string Category;
        public readonly Exception Exception;

        public LogMessage(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            string captegory,
            string message,
            Exception exception)
        {
            this.Timestamp = timestamp;
            this.LogLevel = logLevel;
            this.Category = captegory;
            this.Message = message;
            this.Exception = exception;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            builder.Append(" [");
            builder.Append(LogLevel.ToString());
            builder.Append("] ");
            builder.Append(Category);
            builder.Append(": ");
            builder.AppendLine(Message);

            if (Exception != null)
            {
                builder.AppendLine(Exception.ToString());
            }

            return builder.ToString();
        }
    }
}
