using System;
using Castle.MicroKernel.Registration;
using Xunit;
using Castle.Windsor;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class WhenTestingConfigurationOptions : IDisposable {
        private Type _expectedExceptionDuringReg;
        private QuestionOfLifeUniverseAndEverything _question;
        protected Func<OptionalPropertyInjectionFacility> create_facility = () => new OptionalPropertyInjectionFacility(false);
        protected Func<ComponentRegistration<QuestionOfLifeUniverseAndEverything>, ComponentRegistration<QuestionOfLifeUniverseAndEverything>>
            additonal_registration = x => x;

        public void Dispose() {
            var container = new WindsorContainer();
            container.AddFacility("optional-property-injection", create_facility());
            container.Register(Component.For<AnswerToLifeUniverseAndEverything>());
            RegisterComponent(container);
            Act(container);
            check();
        }
        private void RegisterComponent(WindsorContainer container) {
            try {
                container.Register(additonal_registration(Component.For<QuestionOfLifeUniverseAndEverything>()));
                if (_expectedExceptionDuringReg.IsNotNull())
                    Assert.True(false, "Expected {0} to be thrown".Use(_expectedExceptionDuringReg.FullName));
            }
            catch (Exception ex) {
                if (_expectedExceptionDuringReg.IsNull())
                    throw;
                if (!_expectedExceptionDuringReg.IsAssignableFrom(ex.GetType()))
                    throw;
            }
        }
        private void Act(IWindsorContainer container) {
            if (_expectedExceptionDuringReg.IsNotNull()) return;
            _question = container.Resolve<QuestionOfLifeUniverseAndEverything>();
        }
        protected Action check = delegate { };

        protected void no_properties_were_injected() {
            Assert.Null(_question.TheAnswer);
            Assert.Null(_question.OtherAnswer);
        }
        protected void all_properties_were_injected() {
            Assert.NotNull(_question.TheAnswer);
            Assert.NotNull(_question.OtherAnswer);
        }
        protected void expect_error_while_registering<EXCEPTION_TYPE>() where EXCEPTION_TYPE : Exception {
            _expectedExceptionDuringReg = typeof(EXCEPTION_TYPE);
        }
    }
}
