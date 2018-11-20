namespace Settlers.Toolbox.Infrastructure.Logging
{
    public enum LogLevel
    {
        /// <summary>
        /// Tracing information and debugging minutiae. Generally only switched on in unusual situations.
        /// </summary>
        Verbose,

        /// <summary>
        /// Internal control flow and diagnostic state dumps to facilitate pinpointing of recognised problems.
        /// </summary>
        Debug,

        /// <summary>
        /// Events of interest or that have relevance to outside observers. The default enabled minimum logging level.
        /// </summary>
        Information,

        /// <summary>
        /// Indicators of possible issues or service/functionality degradation.
        /// </summary>
        Warning,

        /// <summary>
        /// Indicating a failure within the application or connected system.
        /// </summary>
        Error,

        /// <summary>
        /// Critical errors causing complete failure of the application.
        /// </summary>
        Fatal
    }
}