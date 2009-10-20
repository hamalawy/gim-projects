using System;
using NUnit.Framework;

namespace Tutorial.RhinoMocks.BDD {
	[TestFixture]
	public abstract class GivenWhenThenTests {
		[SetUp]
		public void Setup() {
			given();
			when();
		}
		protected virtual void given() { }
		protected abstract void when();
	}
}
