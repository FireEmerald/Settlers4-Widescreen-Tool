using System;

namespace Settlers.Toolbox.Infrastructure
{
    public class Result
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public Exception ThrownException { get; private set; }

        public Result()
        {
            SetProperties(true, string.Empty, null);
        }

        public Result(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage)) throw new ArgumentNullException(nameof(errorMessage));

            SetProperties(false, errorMessage, null);
        }

        public Result(string errorMessage, Exception thrownException)
        {
            if (string.IsNullOrEmpty(errorMessage)) throw new ArgumentNullException(nameof(errorMessage));
            if (thrownException == null) throw new ArgumentNullException(nameof(thrownException));

            SetProperties(false, errorMessage, thrownException);
        }

        private void SetProperties(bool success, string errorMessage, Exception thrownException)
        {
            Success = success;
            ErrorMessage = errorMessage;
            ThrownException = thrownException;
        }
    }
}