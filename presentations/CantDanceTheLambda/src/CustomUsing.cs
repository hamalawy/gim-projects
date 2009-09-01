using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;

namespace CantDanceTheLambda {
    public class CustomUsing<T> where T : IDisposable {
        T _disposableObject;
        public CustomUsing(T disposableObject) {
            _disposableObject = disposableObject;
        }
        public void Do(Action<T> action) {
            try {
                action(_disposableObject);
            }
            finally {
                _disposableObject.Dispose(); 
            }
        }
    }
    public class TestDisposable : IDisposable {
        public TestDisposable() {
            DisposeCalled = false;
        }
        public bool DisposeCalled { get; private set; }
        public void Dispose() {
            DisposeCalled = true;
        }
    }
    [TestFixture]
    public class CustomUsingTests {
       
        [Test] public void Call_dispose_if_action_succeeds() {
            var disposableObject = new TestDisposable();
            new CustomUsing<TestDisposable>(disposableObject).Do(d => {
                Console.WriteLine("Things are being done to TestDisposable d");
            });
            Assert.IsTrue(disposableObject.DisposeCalled);
        }

        [Test] public void Call_dispose_if_action_fails() {
            var disposableObject = new TestDisposable();
            try {
                new CustomUsing<TestDisposable>(disposableObject).Do(d => {
                    throw new InvalidOperationException("An error occurrs!");
                });
            }
            catch (InvalidOperationException) { }
            Assert.IsTrue(disposableObject.DisposeCalled);
        }
    }
}
