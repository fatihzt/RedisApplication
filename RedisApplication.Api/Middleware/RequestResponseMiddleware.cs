using Microsoft.IO;
using Newtonsoft.Json;

namespace RedisApplication.Api.Middleware
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly string _logRequestFilePath; // Log dosyasının yolu
        private readonly string _logResponseFilePath;

        public RequestResponseMiddleware(RequestDelegate next, ILogger<RequestResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _logRequestFilePath = "C:\\Users\\muham\\source\\Repos\\RedisApplication\\RedisApplication.Core\\Log\\RequestLog.txt";
            _logResponseFilePath = "C:\\Users\\muham\\source\\Repos\\RedisApplication\\RedisApplication.Core\\Log\\ResponseLog.txt";
        }
        public async Task Invoke(HttpContext context)
        {

            await LogRequest(context);//request
            ///////////////////
            //response occuring
            await _next(context);
            ///////////////////
            await LogResponse(context);//response
        }

        //stackoverflow 
        private async Task LogRequest(HttpContext context)
        {
            _logger.LogInformation("Request is Occuring");
            context.Request.EnableBuffering();
            var request = context.Request;
            request.Headers.ContentType = "application/json";
            _logger.LogInformation(request.Headers.Authorization);
            _logger.LogInformation(request.ContentType);

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var Request = new Dictionary<string, string>(){
                {"Http Request Information", $"{DateTime.UtcNow}"},
                {"ID", Guid.NewGuid().ToString()},
                {"IP", $"{context.Connection.RemoteIpAddress}" },
                {"Schema", context.Request.Scheme},
                {"Path", context.Request.Path},
                {"QueryString", $"{context.Request.QueryString}"},
                {"Request Body", ReadStreamInChunks(requestStream)}
            };
            var requestJsno = JsonConvert.SerializeObject(Request, Formatting.None);

            _logger.LogInformation(requestJsno);
            await WriteToLogFile(requestJsno,_logRequestFilePath);
            context.Request.Body.Position = 0;

        }
        private async Task WriteToLogFile(string log, string filePath)
        {
            await using (var writer = new StreamWriter(filePath, true))
            {
                await writer.WriteLineAsync(log);
            }
        }

        private async Task LogResponse(HttpContext context)
        {
            _logger.LogInformation("Response is Occuring");
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            if (!context.Response.HasStarted)
            {
                await _next(context);
            }
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var Response = new Dictionary<string, string>(){
                {"Http Response Information", $"{DateTime.UtcNow}"},
                {"ID", Guid.NewGuid().ToString()},
                {"IP", $"{context.Connection.RemoteIpAddress}" },
                {"Schema", context.Request.Scheme},
                {"Path", context.Request.Path },
                {"QueryString", $"{context.Request.QueryString}"},
                {"Response Body", text}
            };

            var responseJson = JsonConvert.SerializeObject(Response, Formatting.None);

            _logger.LogInformation(responseJson);
            await WriteToLogFile(responseJson,_logResponseFilePath);

            await responseBody.CopyToAsync(originalBodyStream);
        }
        //medium
        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}
