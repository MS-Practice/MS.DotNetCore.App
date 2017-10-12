using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace NetCoreControllers.Demo
{
    public class CustomActionNameAttribute : Attribute, IActionModelConvention
    {
        private readonly string _actionName;
        public CustomActionNameAttribute(string actionName)
        {
            _actionName = actionName;
        }

        public void Apply(ActionModel action)
        {
            action.ActionName = _actionName;
        }
    }
}
