using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Extantions;
using FarmAppServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace FarmAppServer.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, FarmAppContext appContext, ICustomLogger logger)
        {
            var originalBody = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await FormatRequest(context, logger.Log);
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger.Log);
            }
            finally
            {
                await FinallyWriteBody(appContext, logger.Log, originalBody, responseBody, context);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, Log log)
        {
            ResponseBody result;
            if (ex is DbUpdateException efException)
            {
                log.StatusCode = 400;
                result = new ResponseBody { Header = "Ошибка", Result = efException.InnerException?.Message ?? efException.Message };
            }
            else
            {
                log.StatusCode = 500;
                result = new ResponseBody { Header = "Неизвестная ошибка!", Result = "Сообщите об этой ошибке разработчику!" }; 
            }

            log.Exception += ex.ToString();
            log.ResponseId = result.Id;
            log.ResponseTime = result.ResponseTime;
            log.Header = result.Header;
            log.Result = result.Result;

            context.Response.StatusCode = log.StatusCode.Value;
            context.Response.ContentType = "application/json; charset=utf-8";
            
            return context.Response.WriteAsync(result.ToString());
        }

        private async Task FinallyWriteBody(FarmAppContext ctx, Log log, Stream originalBody, MemoryStream responseBody, HttpContext context)
        {
            try
            {
                await FormatResponse(context.Response, log);
                await responseBody.CopyToAsync(originalBody);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex, log);
            }
            finally
            {
                ctx.Logs.Add(log);
                responseBody.Dispose();
                await ctx.SaveChangesAsync();
            }
        }

        private async Task FormatRequest(HttpContext context, Log logger)
        {
            logger.UserId = context.User.Claims?.FirstOrDefault(c => c.Type == "UserId")?.Value;
            logger.RoleId = context.User.Claims?.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            
            logger.RequestTime = DateTime.Now;
            logger.MethodRoute = context.Request.GetEncodedUrl();

            logger.HttpMethod = context.Request.Method;
            logger.PathUrl = context.Request.Path;

            var stringBuilder = new StringBuilder();
            context.Request.Headers.ToList().ForEach(row =>
            {
                stringBuilder.Append($"{row.Key}: {row.Value} {Environment.NewLine}");
            });

            logger.HeaderRequest = stringBuilder.ToString();
            
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            logger.Param = body?.Length > 4000 ? body.Substring(0, 4000) : body;
        }
        private async Task FormatResponse(HttpResponse response, Log log)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string textResponse = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            //if (textResponse.TryParseJson(out ResponseBody responseBody, out string errorMsg))
            //{
            log.ResponseId = Guid.NewGuid();
            log.Header = "Ok";
                //log.Result = responseBody?.Result?.Length > 4000 ? responseBody.Result.Substring(0, 4000) : responseBody?.Result;
            //}
            //else
            //{
            log.Result = textResponse?.Length > 400 ? textResponse.Substring(0, 4000) : textResponse;
            //}
            log.StatusCode = response.StatusCode;

            //if (!string.IsNullOrWhiteSpace(errorMsg))
            //{
            //    log.Exception += $"JSON parse response: {errorMsg} {Environment.NewLine}";
            //    log.Header = "Exception";
            //}

            StringBuilder stringBuilder = new StringBuilder();
            response.Headers.ToList().ForEach(row =>
            {
                stringBuilder.Append($"{row.Key}: {row.Value} {Environment.NewLine}");
            });

            log.HeaderResponse = stringBuilder.ToString();
            log.FactTime = DateTime.Now;
        }
    }
}
