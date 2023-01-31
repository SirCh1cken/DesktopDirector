using DesktopDirector.App.Model;
using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.ArduinoInterface.Services;
using DesktopDirector.Plugins.Model;
using DesktopDirector.Plugins.Services;
using System.Reflection;

namespace DesktopDirector.App
{
    public class DesktopDirectorApp
    {
        private readonly ArduinoEventService arduinoEventService;
        private readonly IEnumerable<PluginDescriptor> pluginDescriptors;

        public DesktopDirectorApp()
        {
            this.arduinoEventService = new ArduinoEventService();
            this.pluginDescriptors = new PluginDiscoveryService().DiscoverPlugins();
            Initialise();
        }

        public IEnumerable<ComponentConfiguration> Components { get; private set; }
        public IEnumerable<PluginDescriptor> PluginDescriptors
        {
            get
            {
                return pluginDescriptors;
            }
        }

        private void Initialise()
        {
            Components = arduinoEventService.RequestConfiguration().Select(comp => new ComponentConfiguration { Component = comp });

            // Need to set this to start listening on background thread
            //arduinoEventService.StartListening();
        }
    }
}