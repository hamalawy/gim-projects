using System;

namespace Tutorial.RhinoMocks {
	public class Contents {
		public Quantity Book { get; set; }
	}
        public interface IPhysicals {
			Quantity GetLastFor(Contents contents, PhysicalType physicalType);
		}
		public enum PhysicalType {
			Manual, Automatic
		}
		public class Quantity {
			public double Amount { get; set; }
			public string UnitOfMeasure { get; set; }
		}
		public interface IEventLogger {
			void Log(string message);
		}
		public class VarianceCalculator {
			IPhysicals _physicals;
			IEventLogger _log;
			public VarianceCalculator(IPhysicals physicals, IEventLogger events) {
				_physicals = physicals;
				_log = events;
			}

			public Quantity GetVariance(Contents contents) {
				Quantity p = _physicals.GetLastFor(contents, PhysicalType.Manual);
				if (p == null)
					return null;
				var variance = new Quantity() { Amount = p.Amount - contents.Book.Amount };
				_log.Log(String.Format("New variance calculated: {0} - {1} = {2}", p.Amount, contents.Book.Amount, variance.Amount));
				return variance;
			}
		}
}