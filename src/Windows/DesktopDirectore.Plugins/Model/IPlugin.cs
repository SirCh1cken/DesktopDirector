using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.ArduinoInterface.Services;

namespace DesktopDirector.Plugins.Model
{
    public interface IPlugin
    {
        void Execute(Message message, ArduinoEventService arduinoEventService, string componentName);

        string Configuration { set; }
    }
}