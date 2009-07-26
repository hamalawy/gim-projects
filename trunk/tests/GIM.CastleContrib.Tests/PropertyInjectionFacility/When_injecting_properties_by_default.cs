using System;
using Castle.MicroKernel.Registration;
using Xunit;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class When_injecting_properties_by_default : PropertyInjectionScenario {
        public When_injecting_properties_by_default() : base(true) { }
        [Fact] public void will_fill_properties() {
            additonal_registration = x => x;
            check = all_properties_were_injected;
        }
    }
}
