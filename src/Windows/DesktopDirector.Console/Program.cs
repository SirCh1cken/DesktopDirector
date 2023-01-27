using System.IO.Ports;
using System.Text.Json;
using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.ArduinoInterface.Services;
using DesktopDirector.AudioDeviceCmdlets.Service;
using DesktopDirectore.Plugins;
using DesktopDirectore.Plugins.Audio;

namespace DesktopDirector.Console
{
    class Program
    {
        private static Dictionary<string, IPlugin> pluginMapping = new Dictionary<string, IPlugin> {
            { "button0", new DefaultAudioDeviceSelection("Headphones - Wireless (ATH-G1WL)") },
            { "button1", new CommunicationAudioDeviceSelection("Headphones - Wireless (ATH-G1WL)") },
            { "button2", new DefaultAudioDeviceSelection("Headphones - Wired (6- Samson Q2U Microphone)") },
            { "button3", new CommunicationAudioDeviceSelection("Headphones - Wired (6- Samson Q2U Microphone)") },
            { "button4", new DefaultAudioDeviceSelection("Mixer CH1 (2- USB Audio CODEC )") },
            { "button5", new CommunicationAudioDeviceSelection("Mixer CH1 (2- USB Audio CODEC )") }
        };
        static void Main(string[] args)
        {
            //var audioDeviceService = new AudioDeviceService();
            //var devices = audioDeviceService.GetAudioDevices().ToList();
            //foreach (var device in devices)
            //{
            //    System.Console.WriteLine(device.Name);
            //}

            System.Console.WriteLine("Listening to messages");
            var arduinoEventService = new ArduinoEventService();
            arduinoEventService.MessageRecieved += ArduinoEventService_MessageRecieved;
            arduinoEventService.StartListening();
        }

        private static void ArduinoEventService_MessageRecieved(object? sender, ArduinoMessageArgs e)
        {
            if(pluginMapping.ContainsKey(e.Message.Input))
            {
                var plugin = pluginMapping[e.Message.Input];
                plugin.Execute(e.Message);

            }
        }
    }
}