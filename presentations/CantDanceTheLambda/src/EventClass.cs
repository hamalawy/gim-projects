// Author: George Mauer
// Code samples for presentation You Can't Dance the Lambda
// Slide: Time For Review
// Demonstrates how to declare, subscribe to and invoke events.  Demonstrates there is no difference between () and .Invoke()

using System;
using NUnit.Framework;

namespace CantDanceTheLambda {
    public class EventClass {
        public event EventHandler OnSomeEvent;
        public void RaiseEventTwice() {
            if (OnSomeEvent == null) return; 
            OnSomeEvent.Invoke(this, new EventArgs());
            OnSomeEvent(this, new EventArgs());
        }
    }
    [TestFixture]
    public class EventUser {
        [Test]
        public void Can_subscribe_to_event(){
            int eventRaisedCount = 0;
            var ec = new EventClass();
            ec.OnSomeEvent += (o, e) => eventRaisedCount++;
            ec.RaiseEventTwice();
            Assert.AreEqual(2, eventRaisedCount);
        }

    }
}
