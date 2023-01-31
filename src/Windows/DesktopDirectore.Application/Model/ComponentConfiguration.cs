using DesktopDirector.ArduinoInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDirector.App.Model
{
    public class ComponentConfiguration
    {
        public ComponentConfiguration() { 
            Plugins = new List<PluginConfiguration>();
        }
        public Component Component { get; set; }
        public IList<PluginConfiguration> Plugins { get; set; }
    }
}
