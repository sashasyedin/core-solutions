using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSolutions.Common.Models
{
    /// <summary>
    /// Base class for all enumerations.
    /// </summary>
    /// <remarks>
    /// Design taken from Jimmy Bogard - Crafting Wicked Domain Models:
    /// https://vimeo.com/43598193
    /// </remarks>
    [Serializable]
    public abstract class Enumeration : IComparable
    {
        #region Protected Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        protected Enumeration()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="displayName">The display name.</param>
        protected Enumeration(int value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Absolutes the difference.
        /// </summary>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        /// <returns>
        /// The difference.
        /// </returns>
        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        /// <summary>
        /// Gets the enumeration item from the specified display name.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="displayName">The display name.</param>
        /// <returns>
        /// An instance of the type.
        /// </returns>
        public static T FromDisplayName<T>(string displayName)
            where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        /// <summary>
        /// Gets the enumeration item from the specified display value.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An instance of the type.
        /// </returns>
        public static T FromValue<T>(int value)
            where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        /// <summary>
        /// Returns true if the enumeration item is valid.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="displayName">The display name.</param>
        /// <returns>
        /// <c>true</c> if the specified display name is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValid<T>(string displayName)
            where T : Enumeration
        {
            var matchingItem = ListAll<T>().FirstOrDefault(i => i.DisplayName == displayName);
            return matchingItem != null;
        }

        /// <summary>
        /// Returns true if the enumeration item is valid.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if the specified value is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValid<T>(int value)
            where T : Enumeration
        {
            var matchingItem = ListAll<T>().FirstOrDefault(i => i.Value == value);
            return matchingItem != null;
        }

        /// <summary>
        /// Lists all the "enumeration" items.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <returns>
        /// A list of all of the values.
        /// </returns>
        public static IEnumerable<T> ListAll<T>()
            where T : Enumeration
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(info => info.GetValue(null)).OfType<T>();
        }

        /// <summary>
        /// Lists all the "enumeration" items.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// A list of all of the values.
        /// </returns>
        public static IEnumerable<T> ListAll<T>(Func<T, bool> selector)
            where T : Enumeration
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(info => info.GetValue(null)).OfType<T>().Where(selector);
        }

        /// <summary>
        /// Lists all the "enumeration" items.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// A dictionary of all of the values.
        /// </returns>
        public static IDictionary<int, string> ListAll(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(info => info.GetValue(null))
                .ToDictionary(obj => (obj as Enumeration).Value, obj => (obj as Enumeration).DisplayName);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        /// An indications if the objects are equal.
        /// </returns>
        public virtual int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration)other).Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return DisplayName;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <typeparam name="K">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="description">The description.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// The parsed value as type T.
        /// </returns>
        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate)
            where T : Enumeration
        {
            var matchingItem = ListAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        #endregion Private Methods
    }
}
