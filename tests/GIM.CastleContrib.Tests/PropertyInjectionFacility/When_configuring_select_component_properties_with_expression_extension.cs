using System;
using Castle.MicroKernel.Registration;
using Xunit;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class When_configuring_select_component_properties_with_expression_extension : WhenConfiguringWithDSL {
        [Fact]
        public void will_override_the_component_default_and_inject_selected() {
            additonal_registration = reg => reg.WireProperties(o => o.None().Except(x => x.TheAnswer));
            check = q => {
                Assert.NotNull(q.TheAnswer);
                Assert.Null(q.OtherAnswer);
            };
        }
        [Fact]
        public void will_override_the_component_default_and_not_inject_selected() {
            additonal_registration = reg => reg.WireProperties(o => o.All().Except(x => x.OtherAnswer));
            check = q => {
                Assert.NotNull(q.TheAnswer);
                Assert.Null(q.OtherAnswer);
            };
        }
        [Fact]
        public void can_select_multiple_properties() {
            additonal_registration = reg => reg.WireProperties(o => o.None().Except(x => x.TheAnswer).Except(x => x.OtherAnswer));
            check = all_properties_were_injected;
        }
    }
}
