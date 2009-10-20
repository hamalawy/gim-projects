using System;
using NUnit.Framework;

namespace Tutorial.RhinoMocks {
	[TestFixture]
	public abstract class VarianceCalculatorTests {
		protected abstract void BuildMocks(Quantity lastManualPhysical);
		protected abstract VarianceCalculator GetCalculator();
		protected abstract void AssertLoggerWasCalled(bool expectLoggerCalled);

		readonly Contents _contents = new Contents() {
			Book = new Quantity() { Amount = 700 }
		};

		[Test]
		public void Has_physical__Variance_is_physical_minus_book() {
			BuildMocks(new Quantity() { Amount = 1000 });
			var variance = GetCalculator().GetVariance(_contents);
			Assert.That(variance.Amount, Is.EqualTo(300));
		}
		[Test]
		public void Has_physical__Calculation_is_logged() {
			BuildMocks(new Quantity());
			GetCalculator().GetVariance(_contents);
			AssertLoggerWasCalled(true);
		}
		[Test]
		public void Has_no_physical__Variance_is_null() {
			BuildMocks(null);
			var variance = GetCalculator().GetVariance(_contents);
			Assert.IsNull(variance);
		}
		[Test]
		public void Has_no_physical__Calculation_is_not_logged() {
			BuildMocks(null);
			GetCalculator().GetVariance(_contents);
			AssertLoggerWasCalled(false);
		}
	}
}
