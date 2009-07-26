using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Xunit;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class When_configuring_all_component_properties_with_extension_method : WhenTestingConfigurationOptions {
        [Fact]
        public void will_override_the_facility_default_and_inject() {
            create_facility = () => new OptionalPropertyInjectionFacility(false);
            additonal_registration = reg => reg.WireProperties(o => o.All());
            check = all_properties_were_injected;
        }
        [Fact] public void will_override_the_facility_default_and_not_inject() {
            create_facility = () => new OptionalPropertyInjectionFacility(true);
            additonal_registration = reg => reg.WireProperties(o => o.None());
            check = no_properties_were_injected;
        }
        [Fact] public void error_when_configuring_all_and_none_properties_to_inject() {
            additonal_registration = reg => reg.WireProperties(o => o.All().None());
            expect_error_while_registering<InvalidOperationException>();
        }
    }
}
