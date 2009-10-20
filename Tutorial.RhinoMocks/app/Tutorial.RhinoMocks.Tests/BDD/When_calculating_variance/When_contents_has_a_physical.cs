using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tutorial.RhinoMocks.BDD {
	public class When_contents_has_a_physical : When_calculating_variance_spec {
		protected override void given() {
			base.given();
			_physicals.Stub(x => x.GetLastFor(null, PhysicalType.Manual))
				.IgnoreArguments().Return(new Quantity() { Amount = 1000 });
		}
		[Test]
		public void variance_amount_should_be_physical_minus_book() {
			Assert.That(_variance.Amount, Is.EqualTo(300));
		}
		[Test]
		public void calculation_is_logged() {
			_logger.AssertWasCalled(x => x.Log(null), c => c.Constraints(Matches((string m) => !String.IsNullOrEmpty(m))));
		}
	}
}
