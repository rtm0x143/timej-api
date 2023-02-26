using Microsoft.AspNetCore.Mvc;

namespace TimejApi.Helpers
{
    public static class ProblemDetailsExtensions
    {
        public static ActionResult ToActionResult(this ProblemDetails problem) => 
            new ObjectResult(problem) { StatusCode = problem.Status };
    }
}
