namespace Mallos.Networking.Logging.Memory
{
    using System;

    public class MemoryLoggerOptions
    {
        /// <summary>
        /// Gets or sets value indicating if logger accepts new messages.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether scopes should be included in the message.
        /// </summary>
        public bool IncludeScopes { get; set; } = false;

        /// <summary>
        /// Gets or sets the max number of messages saved in memory.
        /// </summary>
        public int MaxMessages
        {
            get => maxMessages;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(MaxMessages)} must be positive.");
                }
                this.maxMessages = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of messages removed if <see cref="MaxMessages"/> are hit.
        /// </summary>
        public int BulkRemoveCount
        {
            get => bulkRemoveCount;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BulkRemoveCount)} must be positive.");
                }
                this.bulkRemoveCount = value;
            }
        }

        private int maxMessages = 1000;
        private int bulkRemoveCount = 1;
    }
}
