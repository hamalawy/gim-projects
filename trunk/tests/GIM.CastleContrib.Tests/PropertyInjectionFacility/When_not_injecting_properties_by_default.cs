using System;
using Castle.MicroKernel.Registration;
using Xunit;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class When_not_injecting_properties_by_default : PropertyInjectionScenario {
        public When_not_injecting_properties_by_default()
            : base(false,
                Component.For<QuestionOfLifeUniverseAndEverything>()) { }
        [Fact] public void will_not_fill_properties() { Assert.Null(_question.TheAnswer); }
    }
    public class When_not_injecting_properties_by_default_but_allowing_specific_property_injection_for_component : PropertyInjectionScenario {
        public When_not_injecting_properties_by_default_but_allowing_specific_property_injection_for_component()
            : base(false,
                Component.For<QuestionOfLifeUniverseAndEverything>().AddAttributeDescriptor("wire-all-properties", "true")) { }
        [Fact] public void will_fill_property() { Assert.NotNull(_question.TheAnswer); }
    }
    public class When_not_injecting_properties_by_default_but_allowing_specific_property_injection_for_component_with_extension : PropertyInjectionScenario {
        public When_not_injecting_properties_by_default_but_allowing_specific_property_injection_for_component_with_extension()
            : base(false,
                Component.For<QuestionOfLifeUniverseAndEverything>().WireProperties(o=>o.All())) { }
        [Fact]
        public void will_fill_property() { Assert.NotNull(_question.TheAnswer); }
    }
}
