using System;

namespace CoreSolutions.Common.Attributes
{
    /// <summary>
    /// An attribute exposing an Enumeration value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EnumAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumAttribute"/> class.
        /// </summary>
        /// <param name="type">The enum type.</param>
        public EnumAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the enum type.
        /// </summary>
        /// <value>
        /// The enum type.
        /// </value>
        public Type Type { get; }
    }
}
