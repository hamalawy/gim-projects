using System;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.Core;

namespace CantDanceTheLambda {
    public class ComponentInstance {
        public ComponentRegistration<S> For<S>() {return Component.For<S>();}
        public ComponentRegistration For(ComponentModel model) {return Component.For(model);}
    }
    public class AllTypesInstance {
        public FromAssemblyDescriptor FromAssembly(Assembly assembly) {return AllTypes.FromAssembly(assembly);}
        public FromAssemblyDescriptor FromAssemblyContaining<T>() {return AllTypes.FromAssemblyContaining<T>();}
        public AllTypesOf Of<T>() {return AllTypes.Of<T>();}
        public AllTypesOf Pick() {return AllTypes.Pick();}
    }
    public class CastleRegistratrionStarter {
        public ComponentInstance Component { get { return new ComponentInstance(); } }
        public AllTypesInstance AllTypes { get { return new AllTypesInstance(); }}
    }
    public static partial class WindsorContainerExtensions {
        public static void Register(this IWindsorContainer container, params Func<CastleRegistratrionStarter, IRegistration>[] registrationTransforms) {
            foreach (var transform in registrationTransforms)
                container.Register(transform(new CastleRegistratrionStarter()));
        }
    }
    public class FluentInterfaceConfig {
        public void StandardCastleDSLRegistration() {
            var container = new WindsorContainer();
            container.Register(
                Component.For<IStrategy>().ImplementedBy<Strategy1>(),
                AllTypes.FromAssemblyContaining<FluentInterfaceConfig>().Where(x=>true)
                    .WithService.FirstInterface()
                );
        }
        public void BetterCastleDSLRegistration() {
            var container = new WindsorContainer();
            container.Register( 
                x=> x.Component.For<IStrategy>().ImplementedBy<Strategy1>(),
                x=> x.AllTypes.FromAssemblyContaining<FluentInterfaceConfig>().Where(t=>true)
                    .WithService.FirstInterface()
                );            
        }
    }
}
