using System.IO.Ports;
using System.Text.Json;
using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.AudioDeviceCmdlets.Service;

namespace DesktopDirector.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var audioDeviceService = new AudioDeviceService();
            var devices = audioDeviceService.GetAudioDevices().ToList();

            for (int i = 0; i < devices.Count(); i++)
            {
                var device = devices[i];
                System.Console.WriteLine($"[{i}] {device.Name} - {device.ID}");
            }

            //SetDefaultAudioDevice(devices[0].ID);
            //SetCommunicationAudioDevice(devices[0].ID);

            SerialPort serialPort;
            serialPort = new SerialPort();
            serialPort.PortName = "COM4";//Set your board COM
            serialPort.BaudRate = 9600;
            serialPort.Open();


            while (true)
            {
                string inputMessage = serialPort.ReadLine();

                if (!string.IsNullOrEmpty(inputMessage))
                {
                    System.Console.WriteLine(inputMessage);
                    try
                    {
                        var message = JsonSerializer.Deserialize<Message>(inputMessage);

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
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Invalid message:" + inputMessage);
                    }



                }
                Thread.Sleep(100);
            }
        }
    }
}