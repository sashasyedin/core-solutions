using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreSolutions.Utils.Extensions
{
    public static class BindingExtensions
    {
        public static T BindId<T>(this T model, Expression<Func<T, Guid>> expression)
            => model.Bind(expression, Guid.NewGuid());

        public static T Bind<T>(this T model, Expression<Func<T, object>> expression, object value)
            => model.Bind<T, object>(expression, value);

        private static TModel Bind<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression, object value)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var propertyName = memberExpression.Member.Name.ToLowerInvariant();
            var modelType = model.GetType();

            var field = modelType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));

            if (field == default)
            {
                return model;
            }

            field.SetValue(model, value);
            return model;
        }
    }
}
