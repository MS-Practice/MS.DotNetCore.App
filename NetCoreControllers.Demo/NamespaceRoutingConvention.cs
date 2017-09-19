using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreControllers.Demo
{
    public class NamespaceRoutingConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var hasRouteAttributes = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);
            if (!hasRouteAttributes
                    && controller.ControllerName.Contains("Namespace")) // affect one controller in this sample
            {
                // Replace the . in the namespace with a / to create the attribute route
                // Ex: MySite.Admin namespace will correspond to MySite/Admin attribute route
                // Then attach [controller], [action] and optional {id?} token.
                // [Controller] and [action] is replaced with the controller and action
                // name to generate the final template
                controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel()
                {
                    Template = controller.ControllerType.Namespace.Replace('.', '/') + "/[controller]/[action]/{id?}"
                };
            }
        }
    }
}
