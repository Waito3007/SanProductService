using SANProductService.Product.Domain.Extensions;

namespace SANProductService.Product.Domain.Exceptions;

/// <summary>
/// Response wrapper chuẩn cho API - Chỉ cần new ApiResponse(data)
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; } = true;
    public int Code { get; set; } = (int)ResponseType.Success;
    public string Message { get; set; } = ResponseType.Success.GetDescription();
    public object? Data { get; set; }
    public object? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Response thành công với data
    /// </summary>
    public ApiResponse(object data)
    {
        Success = true;
        Code = (int)ResponseType.Success;
        Message = ResponseType.Success.GetDescription();
        Data = data;
    }

    /// <summary>
    /// Response với ResponseType cụ thể (không có data)
    /// </summary>
    public ApiResponse(ResponseType responseType)
    {
        Success = (int)responseType < 1000;
        Code = (int)responseType;
        Message = responseType.GetDescription();
    }

    /// <summary>
    /// Response với ResponseType và data
    /// </summary>
    public ApiResponse(object data, ResponseType responseType)
    {
        Success = (int)responseType < 1000;
        Code = (int)responseType;
        Message = responseType.GetDescription();
        Data = data;
    }

    /// <summary>
    /// Response lỗi với errors chi tiết
    /// </summary>
    public ApiResponse(ResponseType responseType, object errors)
    {
        Success = false;
        Code = (int)responseType;
        Message = responseType.GetDescription();
        Errors = errors;
    }
}