using System;
using NUnit.Framework;
using System.IO;

namespace CantDanceTheLambda {
    public class Lazy<T> {
        private bool _instantiated = false;
        private Func<T> _instantiationFunction;
        private T _value;
        public T GetInstance() {
            if (!_instantiated) {
                _value = _instantiationFunction();
                _instantiated = true;
            }
            return _value;
        }
        public Lazy(Func<T> instantiationFunction) {
            _instantiationFunction = instantiationFunction;
        }
        public static implicit operator T(Lazy<T> lazyInstance) {
            return lazyInstance.GetInstance();
        }
    }
    [TestFixture]
    public class LazyLoadingTests {
        private Lazy<Stream> _lazyInstance;
        private int times_object_instantiated;
        [SetUp] public void before_each_test() {
            times_object_instantiated = 0;
            _lazyInstance = new Lazy<Stream>(()=>{
                times_object_instantiated++;
                return new MemoryStream();
            });
        }
        [Test] public void Object_will_be_instantiated_only_after_attempting_to_retrieve_it() {
            Assert.AreEqual(0, times_object_instantiated);
            Stream s = _lazyInstance.GetInstance();
            Assert.AreEqual(1, times_object_instantiated);
            Assert.IsNotNull(s);
        }
        [Test] public void Object_will_not_be_instantiated_twice() {
            Stream s1 = _lazyInstance.GetInstance();
            Assert.AreEqual(1, times_object_instantiated);
            Stream s2 = _lazyInstance.GetInstance();
            Assert.AreEqual(1, times_object_instantiated);
            Assert.AreSame(s1, s2);
        }
        [Test]
        public void Can_work_with_native_value_object() {
            int times_object_instantiated = 0;
            var li = new Lazy<double>(() => {times_object_instantiated++; return 5d;});
            Assert.AreEqual(0, times_object_instantiated);
            Assert.AreEqual(5d, li.GetInstance());
            Assert.AreEqual(1, times_object_instantiated);
            li.GetInstance();
            Assert.AreEqual(1, times_object_instantiated);
        }
        public class StreamContainer {
            private Lazy<Stream> _lazyStream;
            public StreamContainer() {
                _lazyStream = new Lazy<Stream>(() => new MemoryStream());
            }
            public Stream Stream {
                get { return _lazyStream; }
            }
        }
        [Test]
        public void Can_convert_implicitly() {
            var c = new StreamContainer();
            Assert.IsNotNull(c.Stream);
        }
    }
}
