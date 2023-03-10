using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace TimejApi.Helpers.Types
{
    public readonly struct Result<T, ProblemDescr>
    {
        public readonly T? ResultValue = default;
        public readonly ProblemDescr? ProblemDescriptor = default;

        public Result(T result) => ResultValue = result;
        public Result(ProblemDescr descriptor) => ProblemDescriptor = descriptor;

        /// <summary>
        /// Method to safely access members
        /// </summary>
        /// <returns>True when result valid, false otherwise</returns>
        public bool Unpack([NotNullWhen(true)] out T? result, [NotNullWhen(false)] out ProblemDescr? descriptor) =>
            ((result = ResultValue) != null) & ((descriptor = ProblemDescriptor) == null);

    }

    public static class DetailedResult
    {
        public static Result<TResult, ProblemDetails> NotFound<TResult>(string? title = null, string? detail = null) => new(new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Title = title,
            Detail = detail
        });
    }
}
