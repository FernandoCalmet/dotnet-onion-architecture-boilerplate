namespace MyCompany.MyProduct.Core.Shared.MaybeComponent;

public sealed class Maybe<T> : IEquatable<Maybe<T>>
{
    private readonly T _value;

    private Maybe(T value) => _value = value;

    public bool HasValue => !HasNoValue;

    public bool HasNoValue => _value is null;

    public T Value => HasValue
        ? _value
        : throw new InvalidOperationException("The value can not be accessed because it does not exist.");

    public static Maybe<T> None => new(default);

    public static Maybe<T> From(T value) => new(value);

    public static implicit operator Maybe<T>(T value) => From(value);

    public static implicit operator T(Maybe<T> maybe) => maybe.Value;

    public bool Equals(Maybe<T> other)
    {
        if (other is null)
        {
            return false;
        }

        if (HasNoValue && other.HasNoValue)
        {
            return true;
        }

        if (HasNoValue || other.HasNoValue)
        {
            return false;
        }

        return Value.Equals(other.Value);
    }

    public override bool Equals(object obj) =>
        obj switch
        {
            null => false,
            T value => Equals(new Maybe<T>(value)),
            Maybe<T> maybe => Equals(maybe),
            _ => false
        };

    public override int GetHashCode() => HasValue ? Value.GetHashCode() : 0;
}