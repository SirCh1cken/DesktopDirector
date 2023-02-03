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
        private Dictionary<string, IPlugin[]> pluginInstanceMap;

        public DesktopDirectorApp()
        {
            this.arduinoEventService = new ArduinoEventService();
            Initialise();
        }

        public IList<ComponentConfiguration> Components { get; private set; }
        public IList<PluginDescriptor> PluginDescriptors { get; private set; }

        public bool IsListening { get { return arduinoEventService.IsListening; } }

        private void Initialise()
        {
            Components = arduinoEventService.RequestConfiguration().Select(comp => new ComponentConfiguration { Component = comp }).ToList();
            PluginDescriptors = new PluginDiscoveryService().DiscoverPlugins().ToList();
        }

        public void StartListening()
        {
            arduinoEventService.MessageRecieved += HandleMessage;
            UpdatePluginInstanceMap();


            // Need to set this to start listening on background thread
            arduinoEventService.StartListening();
        }

        public void StopListening()
        {
            arduinoEventService.StopListening();
        }
        public void UpdatePluginInstanceMap()
        {
            pluginInstanceMap = new Dictionary<string, IPlugin[]>();
            foreach (var componentConfiguration in Components)
            {
                var componentKey = componentConfiguration.Component.Name;
                var plugins = new List<IPlugin>();
                foreach (var pluginConfiguration in componentConfiguration.Plugins)
                {
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginConfiguration.Plugin.PluginType);
                    plugin.Configuration = pluginConfiguration.Configuration;
                    plugins.Add(plugin);
                }

                pluginInstanceMap.Add(componentKey, plugins.ToArray());

            }
        }

        private void HandleMessage(object? sender, ArduinoMessageArgs e)
        {
            if (pluginInstanceMap.ContainsKey(e.Message.Input))
            {
                var plugins = pluginInstanceMap[e.Message.Input];
                foreach (var plugin in plugins)
                {
                    plugin.Execute(e.Message,arduinoEventService, e.Message.Input);
                }
            }
        }
    }
}