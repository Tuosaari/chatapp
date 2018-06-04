using ChatApp.Lib.General;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ChatApp.Configuration
{
    public static class ServiceInitialization
    {
        /// <summary>
        /// Called once on application startup. All one time initialization should go here
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeServices(this IServiceProvider serviceProvider)
        {
            var toBeInitialized = serviceProvider.GetServices(typeof(IInitializable)).Cast<IInitializable>();
            foreach (var initializable in toBeInitialized) {
                initializable.Initialize().Wait();
            }
        }
    }
}
