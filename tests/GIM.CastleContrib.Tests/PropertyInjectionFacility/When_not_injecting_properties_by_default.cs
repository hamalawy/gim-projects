using System;
using Castle.MicroKernel.Registration;
using Xunit;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class When_not_injecting_properties_by_default : PropertyInjectionScenario {
        public When_not_injecting_properties_by_default() : base(false) { }
        [Fact] public void will_not_fill_properties() {
            additonal_registration = x => x;
            check = no_properties_were_injected; 
        }
        [Fact] public void but_allowing_injection_on_component() {
            additonal_registration = x => x.AddAttributeDescriptor("wire-all-properties", "TRUE");
            check = all_properties_were_injected; 
        }
        [Fact] public void but_allowing_injection_on_component_with_extension() {
            additonal_registration = x => x.WireProperties(o=>o.All());
            check = all_properties_were_injected; 
        }
    }

}
