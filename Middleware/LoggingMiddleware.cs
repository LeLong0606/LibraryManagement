using System.Diagnostics;
using System.Net.WebSockets;

namespace LibraryManagement.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly string _logFilePath;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Middleware");
            Directory.CreateDirectory(folderPath);
            _logFilePath = Path.Combine(folderPath, "log.txt");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();

            var requestTime = DateTime.Now;
            stopwatch.Start();

            var requestMethod = context.Request.Method;
            var requestDomain = context.Request.Host.Value;

            var requestLog = $"[Request] {requestDomain} - {requestMethod} - Request Time: {requestTime:HH:mm:ss.fff}";
            _logger.LogInformation(requestLog);
            AppendLogToFile(requestLog);

            await _next(context);

            stopwatch.Stop();
            var responseTime = DateTime.Now;
            var statusCode = context.Response.StatusCode;
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            var responseLog = $"[Response] {requestDomain} - {requestMethod} - Status Code: {statusCode} - Response Time: {responseTime:HH:mm:ss.fff} ({elapsedMs}ms)";
            _logger.LogInformation(responseLog);
            AppendLogToFile(responseLog);
        }

        private void AppendLogToFile(string message)
        {
            try
            {
                using (var writer = File.AppendText(_logFilePath))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
                }
            }
            catch (IOException ex)
            {
                _logger.LogError($"Error writing log to file: {ex.Message}");
            }
        }
    }
}
