using DesktopDirector.ArduinoInterface.Model;

namespace DesktopDirector.Plugins
{
    public interface IPlugin
    {
        void Execute(Message message);
    }
}