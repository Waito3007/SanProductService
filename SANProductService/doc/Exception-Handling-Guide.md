# Exception Handling System - SANProductService

## Tổng quan

Hệ thống xử lý exception trong SANProductService được thiết kế để tự động bắt và chuyển đổi exception thành response format chuẩn, đồng thời ghi log chi tiết bằng BackgroundLogService.

## Kiến trúc

### 1. ProjectException Class
Custom exception chứa thông tin lỗi business logic:
```csharp
public class ProjectException : Exception
{
    public ResponseType ResponseType { get; }
    public bool Success { get; } = false;

    public ProjectException(ResponseType responseType)
        : base(responseType.GetDescription())
    {
        ResponseType = responseType;
    }

    public ProjectException(ResponseType responseType, string additionalMessage) 
        : base($"{responseType.GetDescription()} - {additionalMessage}")
    {
        ResponseType = responseType;
    }
}
```

### 2. ResponseType Enum
Định nghĩa các loại lỗi và mã lỗi tương ứng:
```csharp
public enum ResponseType
{
    [Description("Tên không được trống")]
    NameCannotBeEmpty = 4001,
    
    [Description("Hình ảnh không được trống")]
    ImageCannotBeEmpty = 4002,
    
    [Description("Không tìm thấy")]
    NotFound = 4004,
    
    [Description("Không có quyền truy cập")]
    Unauthorized = 4401,
    
    [Description("Bị cấm truy cập")]
    Forbidden = 4403,
    
    [Description("Dữ liệu đã tồn tại")]
    Conflict = 4409,
    
    [Description("Lỗi server")]
    InternalServerError = 5000
}
```

### 3. ProductResponse Class
Format response chuẩn cho tất cả API:
```csharp
public class ProductResponse
{
    public bool Success { get; set; } = true;
    public ResponseType? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public object? Data { get; set; }
}
```

### 4. GlobalExceptionMiddleware
Middleware tự động bắt exception và convert thành response:
- Bắt tất cả exception trong request pipeline
- Phân loại exception và map với HTTP status code
- Ghi log chi tiết bằng BackgroundLogService
- Trả về response format chuẩn

## Cách sử dụng

### 1. Trong Service Layer
Throw ProjectException khi có lỗi business logic:
```csharp
public async Task<CreateBrandResponse> CreateBrand(CreateBranchRequest request)
{
    // Validation
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        throw new ProjectException(ResponseType.NameCannotBeEmpty);
    }

    if (string.IsNullOrWhiteSpace(request.LogoUrl))
    {
        throw new ProjectException(ResponseType.ImageCannotBeEmpty);
    }

    // Business logic
    var brand = new Brand
    {
        Id = Guid.NewGuid(),
        Name = request.Name,
        LogoUrl = request.LogoUrl
    };

    await _brandRepository.CreateBrand(brand);
    return new CreateBrandResponse { BrandId = brand.Id };
}
```

### 2. Trong Controller Layer
Chỉ cần gọi service và return success response:
```csharp
[HttpPost("add")]
public async Task<IActionResult> CreateBrand([FromBody] CreateBranchRequest request)
{
    var data = await _brandService.CreateBrand(request);
    
    var response = new ProductResponse
    {
        Success = true,
        Data = data
    };
    
    return Ok(response);
}
```

## Mapping HTTP Status Code

| ResponseType | HTTP Status | Mô tả |
|--------------|-------------|-------|
| NameCannotBeEmpty | 400 Bad Request | Validation lỗi |
| ImageCannotBeEmpty | 400 Bad Request | Validation lỗi |
| NotFound | 404 Not Found | Không tìm thấy resource |
| Unauthorized | 401 Unauthorized | Chưa đăng nhập |
| Forbidden | 403 Forbidden | Không có quyền |
| Conflict | 409 Conflict | Dữ liệu trùng lặp |
| InternalServerError | 500 Internal Server Error | Lỗi server |
| Các exception khác | 500 Internal Server Error | Lỗi không mong muốn |

## Các Case Sử Dụng

### Case 1: Validation Error
**Input:** Name trống trong CreateBrand
```json
{
  "name": "",
  "logoUrl": "https://example.com/logo.png"
}
```

**Output:**
```json
HTTP 400 Bad Request
{
  "success": false,
  "errorCode": "NameCannotBeEmpty",
  "errorMessage": "Tên không được trống",
  "data": null
}
```

**Log được ghi:**
```
BEGIN MESSAGE [2024-12-18 10:30:00.123]
SessionLogId: abc-def-123
Message: Business Logic Error: NameCannotBeEmpty - Tên không được trống
END MESSAGE
```

### Case 2: Success Response
**Input:** Dữ liệu hợp lệ
```json
{
  "name": "Nike",
  "logoUrl": "https://example.com/nike-logo.png"
}
```

**Output:**
```json
HTTP 200 OK
{
  "success": true,
  "errorCode": null,
  "errorMessage": null,
  "data": {
    "brandId": "123e4567-e89b-12d3-a456-426614174000"
  }
}
```

### Case 3: Server Error
**Input:** Database connection failed

**Output:**
```json
HTTP 500 Internal Server Error
{
  "success": false,
  "errorCode": "InternalServerError", 
  "errorMessage": "Đã xảy ra lỗi không mong muốn",
  "data": null
}
```

**Log được ghi:**
```
BEGIN EXCEPTION [2024-12-18 10:30:00.123 SqlException]
SessionLogId: abc-def-123
Method: GlobalExceptionMiddleware.HandleExceptionAsync
Message: Cannot connect to database
Source: SANProductService
Stack Trace: ...
END EXCEPTION

BEGIN MESSAGE [2024-12-18 10:30:00.124]
SessionLogId: abc-def-123
Message: Unhandled Exception: SqlException - Cannot connect to database
END MESSAGE
```

### Case 4: Not Found
**Service Code:**
```csharp
var brand = await _brandRepository.GetByIdAsync(id);
if (brand == null)
{
    throw new ProjectException(ResponseType.NotFound);
}
```

**Output:**
```json
HTTP 404 Not Found
{
  "success": false,
  "errorCode": "NotFound",
  "errorMessage": "Không tìm thấy",
  "data": null
}
```

### Case 5: Conflict (Duplicate Data)
**Service Code:**
```csharp
var existingBrand = await _brandRepository.GetByNameAsync(request.Name);
if (existingBrand != null)
{
    throw new ProjectException(ResponseType.Conflict, "Brand đã tồn tại");
}
```

**Output:**
```json
HTTP 409 Conflict
{
  "success": false,
  "errorCode": "Conflict",
  "errorMessage": "Dữ liệu đã tồn tại - Brand đã tồn tại",
  "data": null
}
```

## Thêm Exception Type Mới

### Bước 1: Thêm vào ResponseType Enum
```csharp
[Description("Email không hợp lệ")]
InvalidEmail = 4003,
```

### Bước 2: Cập nhật HTTP Status Mapping
```csharp
private static int GetHttpStatusCode(ResponseType responseType)
{
    return responseType switch
    {
        // ...existing mappings...
        ResponseType.InvalidEmail => (int)HttpStatusCode.BadRequest,
        _ => (int)HttpStatusCode.InternalServerError
    };
}
```

### Bước 3: Sử dụng trong Service
```csharp
if (!IsValidEmail(request.Email))
{
    throw new ProjectException(ResponseType.InvalidEmail);
}
```

## Best Practices

### DO ✅
- Sử dụng ProjectException cho business logic errors
- Định nghĩa ResponseType với Description rõ ràng
- Để GlobalExceptionMiddleware xử lý tự động
- Controller chỉ return success response
- Service chỉ throw exception khi có lỗi

### DON'T ❌
- Không try-catch trong Controller
- Không trả về error response trực tiếp từ Service
- Không sử dụng generic Exception cho business logic
- Không hardcode error message trong code

## Logging Integration

Hệ thống tự động ghi log với BackgroundLogService:
- **Business Logic Error**: Ghi log warning với thông tin lỗi
- **Unhandled Exception**: Ghi log exception đầy đủ stack trace
- **Session Tracking**: Tự động track theo SessionLogId

Log files được tạo tại:
- `C:\Logs\SANProductService\SANProductService_Log_yyyyMMdd_xxx.txt`
- `C:\Logs\SANProductService\SANProductService_Data_yyyyMMdd_xxx.txt`
