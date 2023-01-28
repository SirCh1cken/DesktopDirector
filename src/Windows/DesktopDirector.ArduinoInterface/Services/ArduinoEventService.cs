using DesktopDirector.ArduinoInterface.Model;
using System;
using System.Collections.Generic;
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
        public ArduinoEventService() 
        {
        }

        public event EventHandler<ArduinoMessageArgs> MessageRecieved;

        public Component[] RequestConfiguration()
        {

            using(var port = new SerialPort())
            {
                port.PortName = "COM4";
                port.BaudRate = 9600;
                port.Open();

                port.WriteLine("request-configuration");
                Component[] configuration = null;
                for (var i = 0; i < 5; i++)
                {
                    string inputMessage = port.ReadLine();
                    try
                    {
                        configuration = JsonSerializer.Deserialize<Component[]>(inputMessage);
                        if(configuration != null)
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
            serialPort = new SerialPort();

            //Set your board COM
            serialPort.PortName = "COM4";
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
                        MessageRecieved?.Invoke(this, new ArduinoMessageArgs() { Message = message });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }
                //Thread.Sleep(100);
            }
        }

        public void StopListening()
        {
            serialPort.Close();
        }
    }
}
