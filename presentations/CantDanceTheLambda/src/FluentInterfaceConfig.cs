// Author: George Mauer
// Code samples for presentation You Can't Dance the Lambda
// Slide: Easily Navigable DSLs
// Shows how to create Configuration Fluent Interfaces that don't rely on static classes and give you intellisense all the way through

using System;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.Core;

namespace CantDanceTheLambda {
    #region Wrappers of Castle Windsor DSL Static Classes
    public class ComponentInstance {
        public ComponentRegistration<S> For<S>() { return Component.For<S>(); }
        public ComponentRegistration For(ComponentModel model) { return Component.For(model); }
    }
    public class AllTypesInstance {
        public FromAssemblyDescriptor FromAssembly(Assembly assembly) { return AllTypes.FromAssembly(assembly); }
        public FromAssemblyDescriptor FromAssemblyContaining<T>() { return AllTypes.FromAssemblyContaining<T>(); }
        public AllTypesOf Of<T>() { return AllTypes.Of<T>(); }
        public AllTypesOf Pick() { return AllTypes.Pick(); }
    }
    #endregion
    
    /// <summary>
    /// Starting point for registration
    /// </summary>
    public class CastleBetterRegistratrionStartingPoint {
        public ComponentInstance Component { get { return new ComponentInstance(); } }
        public AllTypesInstance AllTypes { get { return new AllTypesInstance(); }}
    }
    
    /// <summary>
    /// The included Register method takes a params array of IRegistration objects.  To get intellisense
    /// all the way through we need to think of it as taking an array of transformations from a starting point
    /// to an IRegistration (the starting point can then include defaults set up by convention)
    /// </summary>
    public static partial class WindsorContainerExtensions {
        public static void Register(this IWindsorContainer container, 
            params Func<CastleBetterRegistratrionStartingPoint, IRegistration>[] registrationTransforms) {
            
            foreach (var transform in registrationTransforms)
                container.Register(transform(new CastleBetterRegistratrionStartingPoint()));
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
