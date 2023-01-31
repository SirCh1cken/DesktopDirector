using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDirector.Plugins.Model
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginNameAttribute : Attribute
    {
        public PluginNameAttribute(string pluginName)
        {
            PluginName = pluginName;
        }

        public string PluginName { get; }
    }
}
