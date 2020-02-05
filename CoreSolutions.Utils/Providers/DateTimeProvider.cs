using CoreSolutions.Utils.Providers.Abstractions;
using System;

namespace CoreSolutions.Utils.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime Today => DateTime.Today;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
