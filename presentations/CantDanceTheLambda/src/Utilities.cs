using System;
using NUnit.Framework;
using Rhino.Mocks;
using System.IO;

namespace CantDanceTheLambda {
    public static class Utilities {
        public static void DoIfNotNull<T>(this T obj, Action<T> act) {
            if (!object.ReferenceEquals(null, obj))
                act(obj);
        }
        public static R IfNotNull<T, R>(this T obj, Func<T, R> getValue) where T : class {
            if (object.ReferenceEquals(null, obj))
                return default(R);
            return getValue(obj);
        }
    }
    [TestFixture]
    public class UtilityTests {
        public interface IThing {
            void DoSomething();
        }
        public class StreamFactory {
            public StreamFactory() {
                Stream = new MemoryStream();
            }
            public Stream Stream { get; set; }
        }
        [Test]
        public void Will_act_on_not_null_object() {
            var thing = MockRepository.GenerateMock<IThing>();
            Utilities.DoIfNotNull(thing, x => x.DoSomething());
            thing.AssertWasCalled(x => x.DoSomething());
        }
        [Test]
        public void Will_not_act_on_null_object() {
            IThing thing = null;
            Utilities.DoIfNotNull(thing, x => x.DoSomething());
        }
        [Test]
        public void Will_return_from_not_null_object() {
            StreamFactory factory = new StreamFactory();
            Assert.IsNotNull(
                factory.IfNotNull(f => f.Stream));
        }
        [Test]
        public void Will_return_null_from_nonexistant_object() {
            StreamFactory factory = null;
            Assert.IsNull(
                factory.IfNotNull(f => f.Stream);
        }
        public void GetLengthOfStream() {
            // TODO
        }
    }
}