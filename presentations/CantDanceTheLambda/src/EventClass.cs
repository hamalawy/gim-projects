using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var ec = new EventClass();
            ec.OnSomeEvent += new EventHandler(HandlerMethod);
            ec.RaiseEventTwice();
        }

        void HandlerMethod(object sender, EventArgs e)
        { }
    }
}
