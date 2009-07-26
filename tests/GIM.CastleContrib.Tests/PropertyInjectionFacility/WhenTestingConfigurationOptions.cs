using System;
using Castle.MicroKernel.Registration;
using Xunit;
using Castle.Windsor;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class WhenTestingConfigurationOptions : IDisposable {
        private Type _expectedExceptionDuringReg;
        protected Func<OptionalPropertyInjectionFacility> create_facility = () => new OptionalPropertyInjectionFacility(false);
        protected Func<ComponentRegistration<QuestionOfLifeUniverseAndEverything>, ComponentRegistration<QuestionOfLifeUniverseAndEverything>>
            additonal_registration = x => x;

        public void Dispose() {
            var container = new WindsorContainer();
            container.AddFacility("optional-property-injection", create_facility());
            container.Register(Component.For<AnswerToLifeUniverseAndEverything>());
            RegisterComponent(container);
            var q = Act(container);
            check(q);
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
        private QuestionOfLifeUniverseAndEverything Act(IWindsorContainer container) {
            if (_expectedExceptionDuringReg.IsNotNull()) return null;
            return container.Resolve<QuestionOfLifeUniverseAndEverything>();
        }
        protected Action<QuestionOfLifeUniverseAndEverything> check = delegate { };

        protected void no_properties_were_injected(QuestionOfLifeUniverseAndEverything question) {
            Assert.Null(question.TheAnswer);
            Assert.Null(question.OtherAnswer);
        }
        protected void all_properties_were_injected(QuestionOfLifeUniverseAndEverything question) {
            Assert.NotNull(question.TheAnswer);
            Assert.NotNull(question.OtherAnswer);
        }
        protected void expect_error_while_registering<EXCEPTION_TYPE>() where EXCEPTION_TYPE : Exception {
            _expectedExceptionDuringReg = typeof(EXCEPTION_TYPE);
        }
    }
}
