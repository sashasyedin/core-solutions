using System;

namespace CoreSolutions.Common.Models
{
    public sealed class WriteOnce<T>
    {
        private bool _hasValue;

        public T Value
        {
            get
            {
                if (_hasValue == false)
                {
                    throw new InvalidOperationException("Value not set");
                }

                return ValueOrDefault;
            }
            set
            {
                if (_hasValue)
                {
                    throw new InvalidOperationException("Value already set");
                }

                ValueOrDefault = value;
                _hasValue = true;
            }
        }

        public T ValueOrDefault { get; private set; }

        public static implicit operator T(WriteOnce<T> value) => value.Value;

        public override string ToString()
            => _hasValue ? Convert.ToString(ValueOrDefault) : string.Empty;
    }
}
