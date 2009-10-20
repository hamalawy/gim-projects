using System;
using Rhino.Mocks;

namespace Tutorial.RhinoMocks.BDD {
	public abstract class When_calculating_variance_spec : GivenWhenThenTests {
		protected VarianceCalculator _calculator;
		protected IPhysicals _physicals;
		protected IEventLogger _logger;

		protected Quantity _variance;
		protected readonly Contents contents = new Contents() { Book = new Quantity() { Amount = 700 } };

		protected override void given() {
			_physicals = MockRepository.GenerateStub<IPhysicals>();
			_logger = MockRepository.GenerateMock<IEventLogger>();

			_calculator = new VarianceCalculator(_physicals, _logger);
		}
		protected override void when() {
			_variance = _calculator.GetVariance(contents);
		}
		protected static Rhino.Mocks.Constraints.AbstractConstraint Matches<T>(Predicate<T> predicate) {
			return Rhino.Mocks.Constraints.Is.Matching<T>(predicate);
		}

	}
}
