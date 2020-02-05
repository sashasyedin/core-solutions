using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSolutions.Utils
{
    /// <summary>
    /// Guard clauses.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures that the specified argument is not empty.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="name">The parameter name.</param>
        public static void NotEmpty<T>(IEnumerable<T> items, string name)
        {
            if (items.Any() == false)
            {
                throw new ArgumentException($"Parameter {name} must contain items");
            }
        }

        /// <summary>
        /// Ensures that the specified argument is not null.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The parameter name.</param>
        public static void NotNull(object instance, string name)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(name, $"Parameter {name} cannot be null");
            }
        }

        /// <summary>
        /// Ensures that the specified argument is not null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="name">The parameter name.</param>
        public static void NotNullOrEmpty<T>(IEnumerable<T> items, string name)
        {
            NotNull(items, name);
            NotEmpty(items, name);
        }

        /// <summary>
        /// Ensures that the specified argument is true.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="name">The parameter name.</param>
        public static void True(bool condition, string name)
        {
            if (condition == false)
            {
                throw new ArgumentException($"Parameter {name} must be true", name);
            }
        }

        #region Extensions

        /// <summary>
        /// Ensures that the specified argument is not null.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The parameter name.</param>
        /// <returns>
        /// The given object.
        /// </returns>
        public static T ThrowIfNull<T>(this T instance, string name)
            where T : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(name, $"Parameter {name} cannot be null");
            }

            return instance;
        }

        #endregion Extensions
    }
}
