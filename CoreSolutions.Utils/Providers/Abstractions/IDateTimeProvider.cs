using System;

namespace CoreSolutions.Utils.Providers.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime Today { get; }
        DateTime UtcNow { get; }
    }
}
