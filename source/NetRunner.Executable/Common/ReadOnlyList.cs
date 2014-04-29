using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Invokation.Functions;

namespace NetRunner.Executable.Common
{
    [ImmutableObject(true)]
    [Pure]
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
            return JoinToStringLazy(", ").ToString();
        }

        public object JoinToStringLazy(string separator)
        {
            return new LazyJoiner(this, separator);
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
    }
}
