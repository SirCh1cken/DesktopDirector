using System.IO.Ports;
using System.Text.Json;
using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.ArduinoInterface.Services;
using DesktopDirector.AudioDeviceCmdlets.Service;

namespace DesktopDirector.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Listening to messages");
            var arduinoEventService = new ArduinoEventService();
            arduinoEventService.MessageRecieved += ArduinoEventService_MessageRecieved;
            arduinoEventService.StartListening();
        }

        private static void ArduinoEventService_MessageRecieved(object? sender, ArduinoMessageArgs e)
        {
            var audioDeviceService = new AudioDeviceService();
            var devices = audioDeviceService.GetAudioDevices().ToList();

            var message = e.Message;
            switch (message.Input)
            {
                case "Button0":
                    if (message.Value == 1)
                    {
                        audioDeviceService.SetDefaultAudioDevice(devices[0].ID);
                    }
                    break;
                case "Button1":
                    if (message.Value == 1)
                    {
                        audioDeviceService.SetCommunicationAudioDevice(devices[0].ID);
                    }
                    break;
                case "Button2":
                    if (message.Value == 1)
                    {
                        audioDeviceService.SetDefaultAudioDevice(devices[1].ID);
                    }
                    break;
                case "Button3":
                    if (message.Value == 1)
                    {
                        audioDeviceService.SetCommunicationAudioDevice(devices[1].ID);
                    }
                    break;
                case "Button4":
                    if (message.Value == 1)
                    {
                        audioDeviceService.SetDefaultAudioDevice(devices[2].ID);
                    }
                    break;
                case "Button5":
                    if (message.Value == 1)
                    {
                        audioDeviceService.SetCommunicationAudioDevice(devices[2].ID);
                    }
                    break;
            }
        }
    }
}