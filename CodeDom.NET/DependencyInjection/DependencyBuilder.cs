using CodeDom.NET.Abstract;
using CodeDom.NET.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDom.NET.DependencyInjection
{
    public static class DependencyBuilder
    {
        public static void RegisterServicesFromPersistance(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder, ModelBuilder>();
        }
    }
}
