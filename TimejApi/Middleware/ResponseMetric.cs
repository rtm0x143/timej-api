using System.Diagnostics;
using TimejApi.Helpers;

namespace TimejApi.Middleware;

public class ResponseMetric
{
    private readonly RequestDelegate _request;

    public ResponseMetric(RequestDelegate request)
    {
        _request = request ?? throw new ArgumentNullException(nameof(request));
    }

    public async Task Invoke(HttpContext httpContext, MetricReporter reporter)
    {
        var path = httpContext.Request.Path.Value;
        if (path == "/metrics")
        {
            await _request.Invoke(httpContext);
            return;
        }
        var sw = Stopwatch.StartNew();

        try
        {
            await _request.Invoke(httpContext);
        }
        finally
        {
            sw.Stop();
            reporter.RegisterRequest();
            reporter.RegisterResponseTime(httpContext.Response.StatusCode, httpContext.Request.Method, sw.Elapsed);
        }
    }
}