using DesktopDirector.ArduinoInterface.Model;

namespace DesktopDirectore.Plugins
{
    public interface IPlugin
    {
        void Execute(Message message);
    }
}