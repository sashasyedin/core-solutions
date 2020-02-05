using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CoreSolutions.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static T GetCustomAttribute<T>(this Enum item)
            where T : Attribute
        {
            var attribute = GetAttribute(item, typeof(T));

            if (attribute != default)
            {
                var customAttribute = (T)attribute;

                if (customAttribute != null)
                {
                    return customAttribute;
                }
            }

            return null as T;
        }

        public static string GetDisplayName(this Enum item, string[] parameters)
        {
            var attribute = GetCustomAttribute<DisplayAttribute>(item);

            if (attribute == default)
            {
                return string.Empty;
            }

            var name = attribute.Name;
            return string.Format(name, parameters);
        }

        private static Attribute GetAttribute(Enum item, Type type)
        {
            var enumType = item.GetType();

            if (enumType != default)
            {
                var fieldInfo = enumType.GetField(item.ToString());

                if (fieldInfo != default)
                {
                    var attribute = fieldInfo.GetCustomAttribute(type, false);

                    if (attribute != default)
                    {
                        return attribute;
                    }
                }
            }

            return null as Attribute;
        }
    }
}
