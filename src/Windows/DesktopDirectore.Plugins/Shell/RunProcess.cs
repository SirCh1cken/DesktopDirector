using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.Plugins.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDirector.Plugins.Shell
{
    [PluginName("Run a program")]
    public class RunProcess : IPlugin
    {
        private string configuration;

        public string Configuration { set => this.configuration = value; }

        public void Execute(Message message)
        {
            if (message.Value == 1)
            {
                Process notepad = new Process();
                notepad.StartInfo.FileName = configuration;
                //notepad.StartInfo.Arguments = "DemoText";
                notepad.Start();
            }
        }
    }
}
