using System;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using System.Linq;

namespace GIM.CastleContrib {

    public class OptionalPropertyInjectionFacility : AbstractFacility, IFacility {
        private bool _provideByDefault;
        public OptionalPropertyInjectionFacility(bool provideByDefault) {
            _provideByDefault = provideByDefault;
        }
        protected override void Init() {
            Kernel.ComponentRegistered += new ComponentDataDelegate(OnComponentRegistered);
        }

        private bool? ToBool(string stringBool) {
            bool res;
            bool success = Boolean.TryParse(stringBool, out res);
            return success.IsNull() ? null : res as bool?;
        }
        void OnComponentRegistered(string key, IHandler handler) {
            var model = handler.ComponentModel;
            if (!ToBool(model.Configuration.Attributes["wire-all-properties"]) ?? false)
                model.Properties.Clear();
        }
    }
}
