using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.ArduinoInterface.Services;

namespace DesktopDirector.App
{
    public class DesktopDirectorApp
    {
        private readonly ArduinoEventService arduinoEventService;

        public DesktopDirectorApp()
        {
            this.arduinoEventService = new ArduinoEventService();
            Initialise();
        }

        public IEnumerable<Component> Components { get; private set; }

        private void Initialise()
        {
            Components = arduinoEventService.RequestConfiguration();
   
            // Need to set this to start listening on background thread
            //arduinoEventService.StartListening();
        }
    }
}