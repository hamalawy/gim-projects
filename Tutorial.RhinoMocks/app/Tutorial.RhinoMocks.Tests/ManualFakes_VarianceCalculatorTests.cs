using System;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Tutorial.RhinoMocks.Tests {
	public class ManualFakes_VarianceCalculatorTests : VarianceCalculatorTests {

		private FakeEventLoggerSpy _logger;
        private FakePhysicalsStub _physicals;
        protected override void BuildMocks(Quantity lastManualPhysical) {
			_physicals = new FakePhysicalsStub(lastManualPhysical);
			_logger = new FakeEventLoggerSpy();
		}

		protected override VarianceCalculator GetCalculator() {
			return new VarianceCalculator(_physicals, _logger);
		}

		protected override void AssertLoggerWasCalled(bool expectLoggerCalled) {
			Assert.That(_logger.WasLogged(new Regex(".+")), Is.EqualTo(expectLoggerCalled));
		}
	}

	public class FakePhysicalsStub : IPhysicals {
		private Quantity _lastPhysical;
		public FakePhysicalsStub(Quantity lastPhysical) {
			_lastPhysical = lastPhysical;
		}
		public Quantity GetLastFor(Contents contents, PhysicalType physicalType) {
			return _lastPhysical;
		}
	}
	public class FakeEventLoggerSpy : IEventLogger {
		private IList<string> _messagesLogged = new List<string>();
		public void Log(string message) {
			_messagesLogged.Add(message);
		}
		public bool WasLogged(Regex expectedMessageMatcher) { 
			return _messagesLogged.Any(m => expectedMessageMatcher.IsMatch(m));
		}
	}

}
