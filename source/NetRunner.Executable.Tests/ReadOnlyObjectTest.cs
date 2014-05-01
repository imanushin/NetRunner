using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Tests
{
    [TestClass]
    public abstract class ReadOnlyObjectTest
    {
        protected virtual bool SkipTesting
        {
            get
            {
                return false;
            }
        }

        protected virtual void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            Assert.AreEqual(expectedParameterName, actualParameterName);
        }

        protected void BaseGetHashCodeTest<T>(ObjectsCache<T> objects)
        {
            if (SkipTesting)
                return;

            foreach (T target in objects)
            {
                Assert.AreEqual(target.GetHashCode(), target.GetHashCode());
            }
        }

        protected void BaseEqualsTest<T>(ObjectsCache<T> objects)
        {
            if (SkipTesting)
                return;

            List<T> items = objects.ToList();//To have indexer for iterations

            for (int currentIndex = 0; currentIndex < items.Count; currentIndex++)
            {
                T currentItem = items[currentIndex];

                for (int otherIndex = 0; otherIndex < items.Count; otherIndex++)
                {
                    T otherItem = items[otherIndex];

                    if (currentIndex == otherIndex)
                    {
                        Assert.AreEqual(currentItem, otherItem);//Check that it really equal by himself
                    }
                    else
                    {
                        Assert.AreNotEqual(currentItem, otherItem);//The same checking will be from the other, one the next loop step
                    }
                }
            }

            if (items.Count <= 1)
                Assert.Inconclusive("Add more objects to check equals method");

            foreach (T target in objects)
            {
                Assert.IsFalse(target.Equals(null));
                Assert.IsFalse(target.Equals("some string"));
            }
        }

        protected void BaseToStringTest<T>(ObjectsCache<T> objects)
        {
            if (SkipTesting)
                return;

            foreach (T targetObject in objects)
            {
                string result = targetObject.ToString();
                Assert.IsNotNull(result);

                bool containedTemplatedCharacters = result.Contains("{#");

                Assert.IsFalse(containedTemplatedCharacters, string.Format("String {0} contains templated characters", result));
            }
        }

        public sealed class ObjectsCache<T> : IEnumerable<T>
        {
            private readonly Lazy<ReadOnlyList<T>> objects;

            public ObjectsCache(Func<IEnumerable<T>> initializer)
            {
                objects = new Lazy<ReadOnlyList<T>>(() => initializer().ToReadOnlyList());
            }

            internal ReadOnlyList<T> Objects
            {
                get
                {
                    return objects.Value;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return objects.Value.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return objects.Value.GetEnumerator();
            }
        }
    }
}
