using System;
using System.ComponentModel;
using System.Reflection;

namespace SANProductService.Product.Domain.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this System.Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}
