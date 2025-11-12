using System.ComponentModel;

namespace SANProductService.Product.Domain.Enum;

public enum MediaType
{
    [Description("Image File")]
    Image = 1,
    [Description("Video File")]
    Video = 2,
    [Description("Document File")]
    Document = 3
}