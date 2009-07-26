﻿using System;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using System.Linq;
using Castle.Core;

namespace GIM.CastleContrib {

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
            var propertiesToRemove = model.Properties.Where(p => ShouldRemove(p.Property, model)).ToArray();
            propertiesToRemove.ForEach(ps =>
                model.Properties.Remove(ps));
        }

        private bool ShouldRemove(System.Reflection.PropertyInfo pi, ComponentModel model) {
            bool? wirePropertiesOfComponentByDefault = ToBool(model.Configuration.Attributes["wire-all-properties"]);
            bool wireThisSpecificProperty = (model.Configuration.Attributes["wire-selected-properties"] ?? "").Split(',')
                .Select(x => x.Trim()).Contains(pi.Name);
            if (wireThisSpecificProperty)
                return false;
            return !(wirePropertiesOfComponentByDefault ?? _wirePropertiesInContainerByDefault);
        }

        private static bool? ToBool(string stringBool) {
            bool res;
            bool success = Boolean.TryParse(stringBool, out res);
            return success == false ? null : res as bool?;
        }

    }
}
