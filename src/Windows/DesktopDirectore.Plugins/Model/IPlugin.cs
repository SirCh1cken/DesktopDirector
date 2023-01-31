using DesktopDirector.ArduinoInterface.Model;

namespace DesktopDirector.Plugins.Model
{
    public interface IPlugin
    {
        void Execute(Message message);
    }
}