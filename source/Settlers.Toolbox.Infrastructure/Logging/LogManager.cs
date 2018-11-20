using System;

using Settlers.Toolbox.Infrastructure.Logging.Logger;
using Settlers.Toolbox.Infrastructure.Logging.Logger.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Logging
{
    /// <summary>
    /// Manager for the currently used <see cref="IInternalLogger"/>.
    /// </summary>
    public sealed class LogManager
    {
        private IInternalLogger _CurrentLogger = new NullLogger();

        /// <summary>
        /// Gets the instance as a singleton.
        /// </summary>
        public static LogManager Instance { get; } = new LogManager();

        /// <summary>
        /// Gets or sets the current logger.
        /// </summary>
        /// <remarks>Use a <see cref="NullLogger"/> if you don't want any logging.</remarks>
        public IInternalLogger CurrentLogger
        {
            get => _CurrentLogger;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                _CurrentLogger = value;
            }
        }
    }
}