using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace NetCoreControllers.Demo
{
    public class MustBeInRouteParameterModelConvention : Attribute, IParameterModelConvention
    {
        public void Apply(ParameterModel parameter)
        {
            if(parameter.BindingInfo == null)
            {
                parameter.BindingInfo = new BindingInfo();
            }
            parameter.BindingInfo.BindingSource = BindingSource.Path;
        }
    }
}
