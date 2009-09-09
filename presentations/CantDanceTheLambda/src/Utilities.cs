// Author: George Mauer
// Code samples for presentation You Can't Dance the Lambda
// Slide: So...Why Do I Care?
// Demonstrates how to use lambdas to abstract away the usage pattern testing for null before using an object
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
        public class StreamContainer {
            public StreamContainer() {
                Stream = new MemoryStream();
            }
            public Stream Stream { get; set; }
        }

        #region DoIfNotNull
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
        #endregion

        #region IfNotNull
        [Test]
        public void Will_return_from_not_null_object() {
            StreamContainer factory = new StreamContainer();
            Assert.IsNotNull(
                factory.IfNotNull(f => f.Stream));
        }
        [Test]
        public void Will_return_null_from_nonexistant_object() {
            StreamContainer factory = null;
            Assert.IsNull(
                factory.IfNotNull(f => f.Stream));
        }
        public void GetLengthOfStream() {
            StreamContainer factory;
            //factory.IfNotNull(f => f.Stream).IfNotNull(s => s.Length);
        }
        #endregion
    }
}