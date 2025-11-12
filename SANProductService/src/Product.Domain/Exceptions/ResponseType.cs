using System.ComponentModel;

namespace SANProductService.Product.Domain.Exceptions;

public enum ResponseType
{
    [Description("The attribute cannot be empty.")]
    AttributeCannotBeEmpty = 1,

    [Description("The entity was not found.")]
    EntityNotFound = 2,

    [Description("The operation is not allowed.")]
    OperationNotAllowed = 3,

    [Description("An unexpected error occurred.")]
    UnexpectedError = 4
}