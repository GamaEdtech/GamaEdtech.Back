namespace GamaEdtech.Common.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public struct ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse([NotNull] ModelStateDictionary modelState)
        {
            if (modelState.ErrorCount == 0)
            {
                return;
            }

            List<Error>? lst = new(modelState.ErrorCount);
            foreach (var key in modelState.Keys)
            {
                var value = modelState[key];
                if (value?.Errors is null)
                {
                    continue;
                }

                for (var i = 0; i < value.Errors.Count; i++)
                {
                    lst.Add(new Error
                    {
                        Value = value.RawValue,
                        Reference = key,
                        Message = value.Errors[i].ErrorMessage,
                    });
                }
            }

            Errors = lst;
        }

        public ApiResponse(IEnumerable<Error>? errors) => Errors = errors;

        public T? Data { get; set; }

        public string? Message { get; set; }

        public string? Action { get; set; }

        public readonly bool Succeeded => Errors is null || !Errors.Any();

        public IEnumerable<Error>? Errors { get; set; }
    }
}
