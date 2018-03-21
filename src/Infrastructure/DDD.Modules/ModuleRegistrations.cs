using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DDD.Modules
{
    public static class ModuleRegistrations
    {

        public static void RegisterModules(IMvcBuilder mvc, Settings settings)
        {
            var projects = settings.Modules.Where(x => x != null);

            if (projects.Any())
            {
                projects.ToList().ForEach(x => mvc.AddApplicationPart(GetAssembly(x.Assembly)));
            }
        }


        private static Assembly GetAssembly(string projectName)
        {
            var assemblyName = GetAssemblyName(projectName);
            return Assembly.Load(assemblyName);
        }

        private static AssemblyName GetAssemblyName(string projectName)
        {
            return new AssemblyName(projectName);
        }
    }
}
