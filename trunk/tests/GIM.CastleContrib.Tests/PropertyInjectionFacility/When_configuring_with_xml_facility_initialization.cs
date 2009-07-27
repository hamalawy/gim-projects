using System;
using Xunit;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class When_configuring_with_xml_facility_initialization : WhenTestingConfigurationOptions {
        public static string initialXmlConfig =
@"
<configuration>
    <facilities>
        <facility id=""optional-property-injection"" 
            type=""GIM.CastleContrib.OptionalPropertyInjectionFacility, GIM.CastleContrib""
            provideByDefault=""{0}""/>
    </facilities>
</configuration>
";
        public When_configuring_with_xml_facility_initialization() {
            add_facility = delegate { };
            register_answer = c => c.AddComponent<AnswerToLifeUniverseAndEverything>();
            register_question = c => c.AddComponent<QuestionOfLifeUniverseAndEverything>();
        }
        private IWindsorContainer CreateContainerWithFacility(bool injectPropertiesByDefault) {
            return new WindsorContainer(new XmlInterpreter(new XmlStringResource(initialXmlConfig.Use(injectPropertiesByDefault))));
        }
        [Fact]
        public void can_set_all_properties_to_inject_by_default() {
            create_container = () => CreateContainerWithFacility(true);
            check = all_properties_were_injected;
        }
        [Fact]
        public void can_set_all_properties_to_not_inject_by_default() {
            create_container = () => CreateContainerWithFacility(false);
            check = no_properties_were_injected;
        }
    }
}
