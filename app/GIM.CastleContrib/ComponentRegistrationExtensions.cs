using System.Linq;
using System.Collections.Generic;
using System;

namespace Castle.MicroKernel.Registration {
    public class WirePropertiesOptions {
        private bool _shouldWireAll = false;
        public bool ShouldWireAll { get { return _shouldWireAll; } }
        public WirePropertiesOptions All() { _shouldWireAll = true; return this;  }
    }
    public static class ComponentRegistrationExtensions {
        public static ComponentRegistration<S> WireProperties<S>(this ComponentRegistration<S> reg, Func<WirePropertiesOptions, WirePropertiesOptions> optionCreation) {
            var opt = optionCreation(new WirePropertiesOptions());
            if (opt.ShouldWireAll)
                return reg.AddAttributeDescriptor("wire-all-properties", "true");
            return reg;
        }
    }
}
