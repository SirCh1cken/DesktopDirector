using DesktopDirector.App.Model;
using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.ArduinoInterface.Services;

namespace DesktopDirector.App
{
    public class DesktopDirectorApp
    {
        private readonly ArduinoEventService arduinoEventService;

        public DesktopDirectorApp()
        {
            this.arduinoEventService = new ArduinoEventService();
            Initialise();
        }

        public IEnumerable<ComponentConfiguration> Components { get; private set; }
        public IEnumerable<Plugin> Plugins
        {
            get
            {
                return new Plugin[] {
                    new Plugin{ Name="Change Default Audio Device" },
                    new Plugin{ Name="Change Communication Audio Device" },
                    new Plugin{ Name="Change Mute/UnMute Audio Device" },
                    new Plugin{ Name="Change Enable/Disable Camera" },
                    new Plugin{ Name="Run Process" },
                    new Plugin{ Name="Send key stroke/s" },
                    new Plugin{ Name="Something for teams" },
                    new Plugin{ Name="Something for OBS" },
                };
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