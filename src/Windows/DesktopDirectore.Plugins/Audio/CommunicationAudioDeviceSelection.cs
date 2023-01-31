﻿using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.AudioDeviceCmdlets.Service;
using DesktopDirector.Plugins.Model;

namespace DesktopDirector.Plugins.Audio
{
    [PluginName("Set Communication Audio Device")]
    public class CommunicationAudioDeviceSelection : IPlugin
    {
        private string deviceName;
        private AudioDeviceService audioDeviceService;
        private string deviceId;

        public CommunicationAudioDeviceSelection()
        {
            this.audioDeviceService = new AudioDeviceService();
        }
        public string Configuration
        {
            set
            {
                this.deviceName = value;
                var devices = audioDeviceService.GetAudioDevices();
                var device = devices.FirstOrDefault(device => device.Name == deviceName);
                if (device == null)
                {
                    throw new ArgumentException($"Could not find device {deviceName}");
                }
                this.deviceId = device.ID;
            }
        }

        public void Execute(Message message)
        {
            // 0 = on because we are reading a low value
            if (message.Value == 0)
            {
                audioDeviceService.SetCommunicationAudioDevice(this.deviceId);
            }
        }
    }
}
