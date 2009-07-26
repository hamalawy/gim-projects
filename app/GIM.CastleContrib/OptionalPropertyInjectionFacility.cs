using System;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using System.Linq;
using Castle.Core;

namespace GIM.CastleContrib {
    public static partial class ComponentModelExtensions {
        public static bool? GetBoolAttribute(this ComponentModel model, string attributeName) {
            return ToBool(model.Configuration.Attributes[attributeName]);
        }
        private static bool? ToBool(string stringBool) {
            bool res;
            bool success = Boolean.TryParse(stringBool, out res);
            return success == false ? null : res as bool?;
        }
    }
    public class OptionalPropertyInjectionFacility : AbstractFacility, IFacility {
        private bool _wirePropertiesInContainerByDefault;

        public OptionalPropertyInjectionFacility(bool provideByDefault) {
            _wirePropertiesInContainerByDefault = provideByDefault;
        }

        protected override void Init() {
            Kernel.ComponentRegistered += new ComponentDataDelegate(OnComponentRegistered);
        }

        void OnComponentRegistered(string key, IHandler handler) {
            var model = handler.ComponentModel;
            CheckErrors(model);
            var propertiesToRemove = model.Properties.Where(p => ShouldRemove(p.Property, model)).ToArray();
            propertiesToRemove.ForEach(ps =>
                model.Properties.Remove(ps));
        }

        private static void CheckErrors(ComponentModel model) {
            //if ((model.GetBoolAttribute("wire-all-properties") ?? false) && (model.GetBoolAttribute("wire-no-properties") ?? false))
            //    throw new InvalidOperationException("Found contradicting Wire All and None flags when setting up property registration for component {0}".Use(model.Service.FullName));
        }
        private bool ShouldRemove(System.Reflection.PropertyInfo pi, ComponentModel model) {
            bool wireThisSpecificProperty = _wirePropertiesInContainerByDefault;
            wireThisSpecificProperty = model.GetBoolAttribute("wire-component-properties")?? wireThisSpecificProperty;
            bool thisPropertyIsAnException = (model.Configuration.Attributes["excepted-properties"] ?? "").Split(',')
                .Select(x => x.Trim()).Contains(pi.Name);
            if(thisPropertyIsAnException)
                wireThisSpecificProperty = !wireThisSpecificProperty;
            return !wireThisSpecificProperty;
        }

        private static bool? ToBool(string stringBool) {
            bool res;
            bool success = Boolean.TryParse(stringBool, out res);
            return success == false ? null : res as bool?;
        }

    }
}
