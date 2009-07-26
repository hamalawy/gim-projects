using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public abstract class PropertyInjectionScenario {
        protected virtual void Act(IWindsorContainer container) {
            _question = container.Resolve<QuestionOfLifeUniverseAndEverything>();
        }
        public PropertyInjectionScenario(bool injectProperties, params IRegistration[] registrations) {
            var container = new WindsorContainer();
            container.AddFacility("optional-property-injection", new OptionalPropertyInjectionFacility(false));
            container.Register(Component.For<AnswerToLifeUniverseAndEverything>());
            container.Register(registrations);
            Act(container);
        }
        protected QuestionOfLifeUniverseAndEverything _question;
    }
}
