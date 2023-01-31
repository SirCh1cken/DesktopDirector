using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopDirector.Plugins.Model;

namespace DesktopDirector.Plugins.Services
{
    public class PluginDiscoveryService
    {
        public IEnumerable<PluginDescriptor> DiscoverPlugins()
        {
            var iPluginType = typeof(IPlugin);
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var pluginTypes = currentAssemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(t => t!= iPluginType && iPluginType.IsAssignableFrom(t)).ToArray();

            var pluginDescriptors = new List<PluginDescriptor>();
            foreach (var pluginType in pluginTypes)
            {
                PluginNameAttribute nameAttribute = pluginType.GetCustomAttributes(typeof(PluginNameAttribute), true)[0] as PluginNameAttribute;
                var name = nameAttribute.PluginName;
                var pluginDescriptor = new PluginDescriptor(name, pluginType);

                pluginDescriptors.Add(pluginDescriptor);
            }
            return pluginDescriptors.OrderBy(pluginDescriptor=> pluginDescriptor.Name);

        }
    }
}
