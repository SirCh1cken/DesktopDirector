using DesktopDirector.ArduinoInterface.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesktopDirector.ArduinoInterface.Services
{
    public class ArduinoEventService : IArduinoEventService
    {
        private SerialPort serialPort;
        private Thread listeningThread;
        private bool keepListening;

        public ArduinoEventService()
        {
        }

        public event EventHandler<ArduinoMessageArgs> MessageRecieved;

        public Component[] RequestConfiguration()
        {

            using (var port = new SerialPort())
            {
                port.PortName = "COM4";
                port.BaudRate = 9600;
                port.ReadTimeout = 1000;
                port.Open();

                port.WriteLine("request-configuration");
                Component[] configuration = null;
                for (var i = 0; i < 5; i++)
                {
                    try
                    {
                        string inputMessage = port.ReadLine();
                        configuration = JsonSerializer.Deserialize<Component[]>(inputMessage);
                        if (configuration != null)
                        {
                            return configuration;
                        }
                    }
                    catch { }
                }

                port.Close();
                return null;
            }
        }

        public void StartListening()
        {
            keepListening = true;
            listeningThread = new Thread(new ThreadStart(Listen));
            listeningThread.Start();
        }
        public void StopListening()
        {
            keepListening = false;
            listeningThread = null;
        }

        public bool IsListening
        {
            get
            {
                return keepListening && (listeningThread != null) && listeningThread.IsAlive;
            }
        }
        private void Listen()
        {
            serialPort = new SerialPort();

            //Set your board COM
            serialPort.PortName = "COM4";
            serialPort.BaudRate = 9600;
            serialPort.ReadTimeout = 50;
            serialPort.Open();
            while (keepListening)
            {
                try
                {
                    string inputMessage = serialPort.ReadLine();
                    if (!string.IsNullOrEmpty(inputMessage))
                    {
                        System.Console.WriteLine(inputMessage);
                        var message = JsonSerializer.Deserialize<Message>(inputMessage);
                        MessageRecieved?.Invoke(this, new ArduinoMessageArgs() { Message = message });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            serialPort.Close();
        }

        public void SendMessageToComponent(string componentName, string message)
        {
            serialPort.WriteLine($"component-message,{componentName},{message}");
        }
    }
}
