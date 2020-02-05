using CoreSolutions.Common.Attributes;
using CoreSolutions.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSolutions.Common.Web.Filters
{
    /// <summary>
    /// Add enum value descriptions to Swagger.
    /// </summary>
    public class EnumOperationFilter : IOperationFilter
    {
        private const string DeclaredPropertiesPropName = "DeclaredProperties";

        /// <summary>
        /// Applies the changes to the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var parameters = context.ApiDescription.ActionDescriptor.Parameters;

            if (parameters == null)
            {
                return;
            }

            foreach (var param in parameters)
            {
                var declaredProperties = param.ParameterType.GetType()
                    .GetProperty(DeclaredPropertiesPropName)
                    .GetValue(param.ParameterType, null);

                if (!(declaredProperties is IEnumerable<PropertyInfo> properties))
                {
                    continue;
                }

                foreach (var prop in properties)
                {
                    var paramName = prop.CustomAttributes
                        .FirstOrDefault(a => a.AttributeType == typeof(FromQueryAttribute))
                        ?.NamedArguments[0].TypedValue.Value
                        ?.ToString();

                    var enumAttribute = prop.CustomAttributes
                        .FirstOrDefault(a => a.AttributeType == typeof(EnumAttribute));

                    if (enumAttribute == null)
                    {
                        continue;
                    }

                    var argument = enumAttribute.ConstructorArguments.FirstOrDefault();
                    var type = (Type)argument.Value;
                    var enumItems = Enumeration.ListAll(type);

                    if (enumItems == null)
                    {
                        continue;
                    }

                    operation.Parameters.FirstOrDefault(p => p.Name == paramName).Description +=
                        Environment.NewLine +
                        Environment.NewLine +
                        string.Join(
                            Environment.NewLine,
                            enumItems.Select(i => $"{i.Key} = {i.Value}"));
                }
            }
        }
    }
}
