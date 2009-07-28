using System;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using System.Linq;
using Castle.Core;
using Castle.Core.Configuration;

namespace GIM.CastleContrib {
    internal static partial class ComponentModelExtensions {
        public static bool? GetBoolAttribute(this ComponentModel model, string attributeName) {
            return model.Configuration.IfNotNull(x=>
                x.Attributes[attributeName].ToBool());
        }
        public static bool? ToBool(this string stringBool) {
            bool res;
            bool success = Boolean.TryParse(stringBool, out res);
            return success == false ? null : res as bool?;
        }
    }
    public class OptionalPropertyInjectionFacility : AbstractFacility, IFacility {
        private bool _wirePropertiesInContainerByDefault;
        public OptionalPropertyInjectionFacility() : this(true) { }
        public OptionalPropertyInjectionFacility(bool provideByDefault) {
            _wirePropertiesInContainerByDefault = provideByDefault;
        }
        public OptionalPropertyInjectionFacility(string provideByDefault) {
            _wirePropertiesInContainerByDefault = true;
        }
        
        protected override void Init() {
            Kernel.ComponentRegistered += new ComponentDataDelegate(OnComponentRegistered);

            if (FacilityConfig.IsNotNull()) {
                if (FacilityConfig.Attributes["provideByDefault"].ToBool().IsNotNull())
                    _wirePropertiesInContainerByDefault = (bool)FacilityConfig.Attributes["provideByDefault"].ToBool();
            }
        }

        void OnComponentRegistered(string key, IHandler handler) {
            var model = handler.ComponentModel;
            var propertiesToRemove = model.Properties.Where(p => ShouldRemove(p.Property, model)).ToArray();
            propertiesToRemove.ForEach(ps =>
                model.Properties.Remove(ps));
        }

        static bool Eq(string s1, string s2) {
            return string.Equals(s1, s2, StringComparison.InvariantCultureIgnoreCase);
        }
        private IConfiguration GetChildNode(string nodeName, ConfigurationCollection configCollection) {
            return configCollection.FirstOrDefault(c => Eq(c.Name, nodeName));
        }
        private bool ShouldRemove(System.Reflection.PropertyInfo pi, ComponentModel model) {
            if (model.Configuration.IsNull())
                return !_wirePropertiesInContainerByDefault;
            return ! (GetChildNode("wire-properties", model.Configuration.Children)
                .IfNotNull(n => {
                    var wireAllOnComponent = n.Attributes["value"].ToBool() ?? true;
                    if (n.Children.Any(c => Eq("except", c.Name) && Eq(pi.Name, c.Attributes["propertyName"])))
                        return (!wireAllOnComponent) as bool?;
                    return wireAllOnComponent as bool?;
                }) ?? _wirePropertiesInContainerByDefault);
        }
    }
}
