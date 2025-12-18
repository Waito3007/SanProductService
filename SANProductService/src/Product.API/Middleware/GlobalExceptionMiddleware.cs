using System.Net;
using System.Text.Json;
using SANProductService.Product.Domain.Exceptions;
using SANProductService.Product.Domain.Extensions;
using BackgroundLogService.Extensions;

namespace SANProductService.Product.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ProductResponse();
        
        // Get BackgroundLogService instances từ request scope
        var logService = context.RequestServices.GetLogService("SANProductService");
        var sessionLogService = context.RequestServices.GetSessionLogService();

        switch (exception)
        {
            case ProjectException projectEx:
                // Xử lý ProjectException - convert thành response format chuẩn
                response.Success = false;
                response.ErrorCode = projectEx.ResponseType;
                response.ErrorMessage = projectEx.ResponseType.GetDescription();
                
                // Set HTTP status code dựa trên loại lỗi
                context.Response.StatusCode = GetHttpStatusCode(projectEx.ResponseType);
                
                // Log business logic error bằng BackgroundLogService
                if (logService != null && sessionLogService != null)
                {
                    await logService.WriteMessageAsync(sessionLogService, 
                        $"Business Logic Error: {projectEx.ResponseType} - {projectEx.Message}");
                }
                break;

            default:
                response.Success = false;
                response.ErrorCode = ResponseType.InternalServerError;
                response.ErrorMessage = "Đã xảy ra lỗi không mong muốn";
                
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                
                // Log unhandled exception bằng BackgroundLogService
                if (logService != null && sessionLogService != null)
                {
                    await logService.WriteExceptionAsync(sessionLogService, exception);
                    await logService.WriteMessageAsync(sessionLogService, 
                        $"Unhandled Exception: {exception.GetType().Name} - {exception.Message}");
                }
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private static int GetHttpStatusCode(ResponseType responseType)
    {
        return responseType switch
        {
            ResponseType.NameCannotBeEmpty => (int)HttpStatusCode.BadRequest,
            ResponseType.ImageCannotBeEmpty => (int)HttpStatusCode.BadRequest,
            ResponseType.NotFound => (int)HttpStatusCode.NotFound,
            ResponseType.Unauthorized => (int)HttpStatusCode.Unauthorized,
            ResponseType.Forbidden => (int)HttpStatusCode.Forbidden,
            ResponseType.Conflict => (int)HttpStatusCode.Conflict,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}
