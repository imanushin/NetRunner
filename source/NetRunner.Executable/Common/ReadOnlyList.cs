using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRunner.Executable.Common
{
    internal sealed class ReadOnlyList<TValue> : BaseReadOnlyObject, IReadOnlyCollection<TValue>
    {
        private readonly TValue[] innerValues;

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
            return string.Format("{0}: {1}", GetType().Name, string.Join(", ", innerValues));
        }
    }
}
