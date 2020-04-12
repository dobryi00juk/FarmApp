//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Microsoft.

//namespace FarmAppServer.Middlewares
//{
//    public class RequestResponseLoggingMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
//        //private readonly Microsoft.IO.RecyclableMemoryStream _recyclableMemoryStreamManager;

//        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
//        {
//            _next = next;
//            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            //code dealing with the request
//            await LogRequest(context);
//            await LogResponse(context);
            
//            //await _next(context);

//            //code dealing with the response
//        }

//        private async Task LogRequest(HttpContext context)
//        {
//            context.Request.EnableBuffering();
//            using var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
//            var body = await reader.ReadToEndAsync();

//            _logger.LogDebug($"Http Request Information:{Environment.NewLine}" +
//                                   $"Schema:{context.Request.Scheme} " +
//                                   $"Host: {context.Request.Host} " +
//                                   $"Path: {context.Request.Path} " +
//                                   $"QueryString: {context.Request.QueryString} " +
//                                   $"Request Body: {body}");

//            context.Request.Body.Position = 0;
//        }

//        private async Task LogResponse(HttpContext context)
//        {
//            var originalBodyStream = context.Response.Body;

//            using var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
//            context.Response.Body = reader;

//            await _next(context);

//            context.Response.Body.Seek(0, SeekOrigin.Begin);
//            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
//            context.Response.Body.Seek(0, SeekOrigin.Begin);
//            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
//                                   $"Schema:{context.Request.Scheme} " +
//                                   $"Host: {context.Request.Host} " +
//                                   $"Path: {context.Request.Path} " +
//                                   $"QueryString: {context.Request.QueryString} " +
//                                   $"Response Body: {text}");
//            await reader.CopyToAsync(originalBodyStream);
//        }
//    };
//}
