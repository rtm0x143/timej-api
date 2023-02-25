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

}
