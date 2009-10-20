using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tutorial.RhinoMocks.BDD {
	public class When_contents_does_not_have_a_physical : When_calculating_variance_spec {
		[Test]
		public void variance_should_be_null() {
			Assert.That(_variance, Is.Null);
		}
		[Test]
		public void calculation_is_not_logged() {
			_logger.AssertWasNotCalled(x => x.Log(null), c => c.IgnoreArguments());
		}
	}
}
