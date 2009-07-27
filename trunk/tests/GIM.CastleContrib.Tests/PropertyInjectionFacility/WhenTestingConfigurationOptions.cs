using System;
using Castle.MicroKernel.Registration;
using Xunit;
using Castle.Windsor;

namespace GIM.CastleContrib.Tests.PropertyInjectionFacility {
    public class WhenConfiguringWithDSL : WhenTestingConfigurationOptions {
        protected Func<OptionalPropertyInjectionFacility> create_in_code_facility =
            () => new OptionalPropertyInjectionFacility(false);
        protected Func<ComponentRegistration<QuestionOfLifeUniverseAndEverything>, ComponentRegistration<QuestionOfLifeUniverseAndEverything>>
            additonal_registration = x => x;
        public WhenConfiguringWithDSL() {
            create_container = () => new WindsorContainer();
            add_facility = c => c.AddFacility("optional-property-injection", create_in_code_facility());
            register_answer = c => c.Register(Component.For<AnswerToLifeUniverseAndEverything>());
            register_question = c => c.Register(additonal_registration(Component.For<QuestionOfLifeUniverseAndEverything>()));
        }
    }
    public class WhenTestingConfigurationOptions : IDisposable {
        private Type _expectedExceptionDuringReg;
        protected Func<IWindsorContainer> create_container = () => {throw new InvalidOperationException("This method must be assigned");};
        protected Action<IWindsorContainer> add_facility = c => {throw new InvalidOperationException("This method must be assigned");};
        protected Action<IWindsorContainer> register_answer = c => {throw new InvalidOperationException("This method must be assigned");};
        protected Action<IWindsorContainer> register_question = c => { throw new InvalidOperationException("This method must be assigned"); };

        public void Dispose() {
            var container = create_container();
            add_facility(container);
            
            RegisterComponents(container);
            var q = Act(container);
            check(q);
        }
        protected virtual void RegisterComponents(IWindsorContainer container) {
            ExpectError(_expectedExceptionDuringReg, () => {
                register_answer(container);
                register_question(container);
            });
        }
        protected void ExpectError(Type error, Action action)
        {
            try {
                action();
                if (error.IsNotNull())
                    Assert.True(false, "Expected {0} to be thrown".Use(error.FullName));
            }
            catch (Exception ex) {
                if (error.IsNull())
                    throw;
                if (!error.IsAssignableFrom(ex.GetType()))
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
