using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace TimejApi.Helpers.Types
{
    public readonly struct UnsuredResult<TResult, TDescriptor>
    {
        public readonly TResult? Result = default;
        public readonly TDescriptor? ProblemDescriptor = default;

        public UnsuredResult(TResult result) => Result = result;
        public UnsuredResult(TDescriptor descriptor) => ProblemDescriptor = descriptor;

        /// <summary>
        /// Method to safely access members
        /// </summary>
        /// <returns>True when result valid, false otherwise</returns>
        public bool Ensure([NotNullWhen(true)] out TResult? result, [NotNullWhen(false)] out TDescriptor? descriptor) =>
            ((result = Result) != null) & ((descriptor = ProblemDescriptor) == null);

    }

    public static class UnsuredDetailedResult
    {
        public static UnsuredResult<TResult, ProblemDetails> NotFound<TResult>(string? title = null, string? detail = null) => new(new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Title = title,
            Detail = detail
        });
    }
}
