using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.Plugins.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDirector.App.Model
{
    public class PluginConfiguration
    {
        public PluginDescriptor Plugin { get; set; }
        public string Configuration { get; set; }
    }
}
