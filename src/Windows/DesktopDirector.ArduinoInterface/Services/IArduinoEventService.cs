namespace DesktopDirector.ArduinoInterface.Services
{
    public interface IArduinoEventService
    {
        void StartListening();
        void StopListening();
        event EventHandler<ArduinoMessageArgs> MessageRecieved;
    }
}