using System;
using NUnit.Framework;

namespace FluentBuild
{
    public class TestBase
    {
        public void TestMethodSetter<T, T2>(T original, Func<T, T> action, Func<T, object> check, T2 arg)
        {
            var result = action.Invoke(original);
            Assert.That(result, Is.SameAs(original));
            Assert.AreEqual(check.Invoke(original), arg);
        }
    }
}