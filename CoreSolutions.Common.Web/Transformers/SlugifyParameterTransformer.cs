using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace CoreSolutions.Common.Web.Transformers
{
    /// <summary>
    /// Handles hyphens in routes.
    /// </summary>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        private const string Pattern = "([a-z])([A-Z])";
        private const string Replacement = "$1-$2";

        /// <summary>
        /// Transforms the specified route value to a string for inclusion in an URI.
        /// </summary>
        /// <param name="value">The route value to transform.</param>
        /// <returns>
        /// The transformed value.
        /// </returns>
        public string TransformOutbound(object value)
        {
            // Slugify value:
            return value == null
                ? default
                : Regex.Replace(value.ToString(), Pattern, Replacement).ToLower();
        }
    }
}
