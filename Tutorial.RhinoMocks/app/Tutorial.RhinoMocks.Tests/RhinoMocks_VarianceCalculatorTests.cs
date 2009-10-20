using System;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace Tutorial.RhinoMocks.Tests {
	public class RhinoMocks_VarianceCalculatorTests : VarianceCalculatorTests {

		private IEventLogger _logger;
		private IPhysicals _physicals;
		protected override void BuildMocks(Quantity lastManualPhysical) {
			_physicals = MockRepository.GenerateStub<IPhysicals>();
			_physicals.Stub(x => x.GetLastFor(Arg<Contents>.Is.Anything, Arg<PhysicalType>.Is.Anything)).Return(lastManualPhysical);
			_logger = MockRepository.GenerateStub<IEventLogger>();
		}

		protected override VarianceCalculator GetCalculator() {
			return new VarianceCalculator(_physicals, _logger);
		}

		protected override void AssertLoggerWasCalled(bool expectLoggerCalled) {
			if (expectLoggerCalled)
				_logger.AssertWasCalled(x => x.Log(null), c => c.Constraints(Is.Matching<string>(m => !String.IsNullOrEmpty(m))));
			else
				_logger.AssertWasNotCalled(x => x.Log(null), c => c.IgnoreArguments());
		}
	}

}
