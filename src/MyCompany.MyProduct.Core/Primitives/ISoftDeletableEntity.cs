namespace MyCompany.MyProduct.Core.Primitives;

public interface ISoftDeletableEntity
{
    DateTime? DeletedOnUtc { get; }
    bool Deleted { get; }
}