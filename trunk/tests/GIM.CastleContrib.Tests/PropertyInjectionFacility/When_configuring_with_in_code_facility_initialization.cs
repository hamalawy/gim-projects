using System;
using Xunit;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class When_configuring_with_in_code_facility_initialization : WhenTestingConfigurationOptions {

        [Fact]
        public void can_set_all_properties_to_inject_by_default() {
            create_facility = () => new OptionalPropertyInjectionFacility(true);
            check = all_properties_were_injected;
        }
        [Fact]
        public void can_set_all_properties_to_not_inject_by_default() {
            create_facility = () => new OptionalPropertyInjectionFacility(false);
            check = no_properties_were_injected;
        }
    }
}
