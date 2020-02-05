using CoreSolutions.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSolutions.Common.Models
{
    public class Response<TErrorResult>
        where TErrorResult : struct, IConvertible
    {
        private readonly IEnumerable<TErrorResult> _successResults;

        public Response()
        {
            if (typeof(TErrorResult).IsEnum)
            {
                if (Enum.TryParse("None", out TErrorResult result))
                {
                    _successResults = Enumerable.Repeat(result, 1);
                }
            }
        }

        public Response(IEnumerable<TErrorResult> successResults)
        {
            _successResults = successResults;
        }

        public TErrorResult ErrorResult { get; set; }
        public string[] ResultMessageParameters { get; set; }

        public string ResultMessage
        {
            get
            {
                if (ErrorResult is Enum)
                {
                    return (ErrorResult as Enum).GetDisplayName(ResultMessageParameters);
                }

                return string.Empty;
            }
        }

        public bool Success => _successResults.Contains(ErrorResult);
    }
}
