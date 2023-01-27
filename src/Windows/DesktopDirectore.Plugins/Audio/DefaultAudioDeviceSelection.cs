﻿using DesktopDirector.ArduinoInterface.Model;
using DesktopDirector.AudioDeviceCmdlets.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDirectore.Plugins.Audio
{
    public class DefaultAudioDeviceSelection : IPlugin
    {
        private string deviceName;
        private AudioDeviceService audioDeviceService;
        private string deviceId;

        public DefaultAudioDeviceSelection(string deviceName)
        {
            this.deviceName = deviceName;
            this.audioDeviceService = new AudioDeviceService();
            
            var devices = audioDeviceService.GetAudioDevices();
            var device = devices.FirstOrDefault(device => device.Name == deviceName);
            if(device == null)
            {
                throw new ArgumentException($"Could not find device {deviceName}");
            }
            this.deviceId = device.ID;
        }

        public void Execute(Message message)
        {
            if(message.Value ==1)
            { }
            audioDeviceService.SetDefaultAudioDevice(this.deviceId);

        }
    }
}