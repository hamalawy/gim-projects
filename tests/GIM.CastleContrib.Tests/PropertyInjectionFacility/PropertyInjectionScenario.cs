using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Xunit;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public abstract class PropertyInjectionScenario : IDisposable {
        protected virtual void Act(IWindsorContainer container) {
            _question = container.Resolve<QuestionOfLifeUniverseAndEverything>();
        }
        public PropertyInjectionScenario(bool injectPropertiesByDefault) {
            _wirePropertiesByDefault = injectPropertiesByDefault;
        }
        public void Dispose() {
            var container = new WindsorContainer();
            container.AddFacility("optional-property-injection", new OptionalPropertyInjectionFacility(_wirePropertiesByDefault));
            container.Register(Component.For<AnswerToLifeUniverseAndEverything>());
            container.Register(additonal_registration(Component.For<QuestionOfLifeUniverseAndEverything>()));
            Act(container);
            check();
        }
        protected QuestionOfLifeUniverseAndEverything _question;
        protected void no_properties_were_injected() {
            Assert.Null(_question.TheAnswer);
            Assert.Null(_question.OtherAnswer);
        }
        protected void all_properties_were_injected() {
            Assert.NotNull(_question.TheAnswer);
            Assert.NotNull(_question.OtherAnswer);
        }
        protected Func<ComponentRegistration<QuestionOfLifeUniverseAndEverything>, ComponentRegistration<QuestionOfLifeUniverseAndEverything>>
            additonal_registration = x => x;
        private bool _wirePropertiesByDefault;
        protected Action check = delegate { };
    }
}
