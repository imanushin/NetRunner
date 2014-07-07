using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Common
{
    [ImmutableObject(true)]
    [System.Diagnostics.Contracts.Pure]
    internal sealed class ReadOnlyList<TValue> : BaseReadOnlyObject, IReadOnlyCollection<TValue>
    {
        private readonly TValue[] innerValues;
        public static readonly ReadOnlyList<TValue> Empty = new ReadOnlyList<TValue>(new TValue[0]);

        public ReadOnlyList(IEnumerable<TValue> values)
        {
            innerValues = values.ToArray();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return innerValues.Cast<TValue>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get
            {
                return innerValues.Length;
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            return innerValues.Cast<object>();
        }

        protected override string GetString()
        {
            return Count.ToString(CultureInfo.InvariantCulture) + ": " + JoinToStringLazy(", ");
        }

        public object JoinToStringLazy(string separator)
        {
            return new LazyJoiner(this, separator);
        }

        public ReadOnlyList<TValue> Concat(params TValue[] otherValues)
        {
            return Concat((IReadOnlyCollection<TValue>)otherValues); //cast to avoid recursive call
        }

        public ReadOnlyList<TValue> Concat(IReadOnlyCollection<TValue> otherValues)
        {
            Validate.ArgumentIsNotNull(otherValues, "otherValues");

            if (!otherValues.Any())
                return this;

            return new ReadOnlyList<TValue>(Enumerable.Concat(this, otherValues));
        }

        private sealed class LazyJoiner
        {
            private readonly Lazy<string> toStringResult;

            public LazyJoiner(ReadOnlyList<TValue> list, string separator)
            {
                toStringResult = new Lazy<string>(() => string.Join(separator, list));
            }

            public override string ToString()
            {
                return toStringResult.Value;
            }
        }

        public TValue this[int index]
        {
            get
            {
                Validate.ArgumentIntLessThan(index, Count, "index");
                Validate.ArgumentIntGreaterOrEqualZero(index, "index");

                return innerValues[index];
            }
        }

        public TValue Second()
        {
            return this[1];
        }

        public void ForEach(Action<TValue> action)
        {
            foreach (TValue innerValue in innerValues)
            {
                action(innerValue);
            }
        }

        public int? IndexOf(Func<TValue, bool> predicate)
        {
            Validate.ArgumentIsNotNull(predicate, "predicate");

            for (int i = 0; i < Count; i++)
            {
                if (predicate(this[i]))
                {
                    return i;
                }
            }

            return null;
        }

        public int? IndexOf(TValue expectedValue, [NotNull] IEqualityComparer<TValue> comparer)
        {
            Validate.ArgumentIsNotNull(comparer, "comparer");

            return IndexOf(v => comparer.Equals(v, expectedValue));
        }
    }
}
