using System;
using System.Collections.Generic;
using System.Linq;

namespace Force.Ddd
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                throw new ArgumentException(
                    $"Invalid comparison of Value Objects of different types: {GetType()} and {obj.GetType()}");
            }

            var valueObject = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator == (ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator != (ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }

    public abstract class ValueObject<T> : ValueObject
    {
        protected readonly T Value;

        protected ValueObject(T value)
        {
            Value = value;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator T(ValueObject<T> value)
            => value.Value;
    }

    public class StringValueObject : ValueObject<string>
    {
        public StringValueObject(string value) : base(value)
        {
        }

        public bool StartsWith(string value)
        {
            return Value.StartsWith(value);
        }

        public override string ToString()
            => Value;
    }
}