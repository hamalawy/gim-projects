using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace Castle.MicroKernel.Registration {
    public class WirePropertiesOptions<COMPONENT_TYPE> {
        public WirePropertiesOptions<COMPONENT_TYPE> Select<PROPERTY_TYPE>(
            Expression<Func<COMPONENT_TYPE, PROPERTY_TYPE>> expresion) {
            string name;

            if (expresion.Body is MemberExpression)
                name = (expresion.Body as MemberExpression).Member.Name;
            else if (expresion.Body is MethodCallExpression)
                name = (expresion.Body as MethodCallExpression).Method.Name;
            else
                throw new InvalidOperationException("Cannot derive member name");

            return Select(name);
        }
        private List<string> _propertyNames = new List<string>();
        public IEnumerable<string> PropertyNames { get { return _propertyNames; } }
        private bool _shouldWireNone = false;
        public bool ShouldWireNone { get { return _shouldWireNone; } }
        private bool _shouldWireAll = false;
        public bool ShouldWireAll { get { return _shouldWireAll; } }
        public WirePropertiesOptions<COMPONENT_TYPE> All() { _shouldWireAll = true; return this; }
        public WirePropertiesOptions<COMPONENT_TYPE> None() { _shouldWireNone = true; return this;
        }
        public WirePropertiesOptions<COMPONENT_TYPE> Select(string propertyName) {
            _propertyNames.Add(propertyName);
            return this;
        }
    }
    public static partial class OptionalPropertyInjectionFacility_ComponentRegistrationExtensions {
        public static ComponentRegistration<S> WireProperties<S>(this ComponentRegistration<S> reg, Func<WirePropertiesOptions<S>, WirePropertiesOptions<S>> optionCreation) {
            var opt = optionCreation(new WirePropertiesOptions<S>());
            if (opt.PropertyNames.FirstOrDefault().IsNotNull())
                reg.AddAttributeDescriptor("wire-selected-properties", String.Join(", ", opt.PropertyNames.ToArray()));
            if (opt.ShouldWireAll)
                reg.AddAttributeDescriptor("wire-all-properties", "true");
            if (opt.ShouldWireNone)
                reg.AddAttributeDescriptor("wire-no-properties", "true");
            return reg;
        }
    }
}
