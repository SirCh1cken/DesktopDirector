using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.ArduinoInterface.Services;
using DesktopDirector.Plugins.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDirector.Plugins.Output
{
    [PluginName("Turn an LED on or off")]

    public class ToggleLed : IPlugin
    {
        private string configuration;

        public string Configuration { set => this.configuration = value; }

        public void Execute(Message message, ArduinoEventService arduinoEventService, string componentName)
        {
            if (message.Value == 1)
            {
                var configurationSegments = configuration.Split(',');
                var targetComponent = configurationSegments[0];
                var componentMessage = configurationSegments[1];

                arduinoEventService.SendMessageToComponent(targetComponent, componentMessage);
            }
        }
    }
}
