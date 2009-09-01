using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;

namespace CantDanceTheLambda {
    public interface IStringsFilter {
        string[] Filter(string[] strings);
    }
    public delegate bool StringTestDelegate(string sender);
    public class NamedDelegateStringsFilter : IStringsFilter {
        private StringTestDelegate _filter;
        public NamedDelegateStringsFilter(StringTestDelegate filter) {
            _filter = filter;
        }
        public string[] Filter(string[] strings) {
            var returns = new List<string>();
            foreach(var s in strings)
                if(_filter.Invoke(s))
                    returns.Add(s);
            return returns.ToArray();
        }
    }
    public class FuncStringsFilter : IStringsFilter {
        private Func<string, bool> _filter;
        public FuncStringsFilter(Func<string, bool> filter) {
            _filter = filter;
        }
        public string[] Filter(string[] strings) {
            return strings.Where(_filter).ToArray();
        }
    }
    [TestFixture]
    public class StringFilterViaDelegateTests {
        private readonly string[] _words = new[] { "dogs", "cats", "vincent van goh" };
        private bool IsPlural(string str) {
            return str.ToUpperInvariant().EndsWith("S");
        }
        [Test] public void Can_filter_with_named_delegate() {
            var filter = new NamedDelegateStringsFilter(new StringTestDelegate(IsPlural));
            Assert.AreEqual(new[] { "dogs", "cats" }, filter.Filter(_words));
        }
        [Test] public void Can_filter_with_named_anonymous_delegate() {
            var filter = new NamedDelegateStringsFilter(delegate(string x) {
                return x.ToUpperInvariant().EndsWith("S");
            });
            Assert.AreEqual(new[] { "dogs", "cats" }, filter.Filter(_words));
        }
        [Test]
        public void Can_filter_with_func_delegate() {
            var filter = new FuncStringsFilter(new Func<string, bool>(IsPlural));
            Assert.AreEqual(new[] { "dogs", "cats" }, filter.Filter(_words));
        }
        [Test] public void Can_filter_with_func_anonymous_delegate() {
            var filter = new FuncStringsFilter(delegate(string x) {
                return x.ToUpperInvariant().EndsWith("S");
            });
            Assert.AreEqual(new[] { "dogs", "cats" }, filter.Filter(_words));
        }
        [Test]
        public void Can_filter_with_lambda() {
            var filter = new FuncStringsFilter(new Func<string, bool>(x => x.ToUpperInvariant().EndsWith("S")));
            Assert.AreEqual(new[] { "dogs", "cats" }, filter.Filter(_words));
        }
    }
}