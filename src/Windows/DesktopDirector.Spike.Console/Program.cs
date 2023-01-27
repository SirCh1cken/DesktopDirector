using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using CoreAudioApi;
using System.IO.Ports;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace RetrieveAudioDeviceInfo
{
    public class Message
    {
        public string Input { get; set; }
        public int Value { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var devices = GetAudioDevices().ToList();

            for (int i = 0; i < devices.Count(); i++)
            {
                var device = devices[i];
                Console.WriteLine($"[{i}] {device.Name} - {device.ID}");
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
                    Console.WriteLine(inputMessage);
                    try
                    {
                        var message = JsonSerializer.Deserialize<Message>(inputMessage);

                        switch (message.Input)
                        {
                            case "Button0":
                                if (message.Value == 1)
                                {
                                    SetDefaultAudioDevice(devices[0].ID);
                                }
                                break;
                            case "Button1":
                                if (message.Value == 1)
                                {
                                    SetCommunicationAudioDevice(devices[0].ID);
                                }
                                break;
                            case "Button2":
                                if (message.Value == 1)
                                {
                                    SetDefaultAudioDevice(devices[1].ID);
                                }
                                break;
                            case "Button3":
                                if (message.Value == 1)
                                {
                                    SetCommunicationAudioDevice(devices[1].ID);
                                }
                                break;
                            case "Button4":
                                if (message.Value == 1)
                                {
                                    SetDefaultAudioDevice(devices[2].ID);
                                }
                                break;
                            case "Button5":
                                if (message.Value == 1)
                                {
                                    SetCommunicationAudioDevice(devices[2].ID);
                                }
                                break;
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Invalid message:" + inputMessage);
                    }



                }
                Thread.Sleep(100);
            }
        }

        static IList<AudioDevice> GetAudioDevices()
        {
            // Create a new MMDeviceEnumerator
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

            // Create a AudioDeviceCreationToolkit
            AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

            MMDeviceCollection DeviceCollection = null;
            try
            {
                // Enumerate all enabled devices in a collection
                DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
            }
            catch
            {
                // Error
                throw new System.Exception("Error in parameter List - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
            }

            var deviceList = new List<AudioDevice>();

            // For every MMDevice in DeviceCollection
            for (int i = 0; i < DeviceCollection.Count; i++)
            {
                // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
                deviceList.Add(new AudioDevice(i + 1, DeviceCollection[i], Toolkit.IsDefault(DeviceCollection[i].ID), Toolkit.IsDefaultCommunication(DeviceCollection[i].ID)));
            }

            return deviceList;
        }
        static AudioDevice GetAudioDevice(string id)
        {
            // Create a new MMDeviceEnumerator
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

            // If the ID parameter received a value
            if (!string.IsNullOrEmpty(id))
            {
                // Create a AudioDeviceCreationToolkit
                AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

                MMDeviceCollection DeviceCollection = null;
                try
                {
                    // Enumerate all enabled devices in a collection
                    DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
                }
                catch
                {
                    // Error
                    throw new System.Exception("Error in parameter ID - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
                }

                // For every MMDevice in DeviceCollection
                for (int i = 0; i < DeviceCollection.Count; i++)
                {
                    // If this MMDevice's ID is the same as the string received by the ID parameter
                    if (string.Compare(DeviceCollection[i].ID, id, System.StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
                        return (new AudioDevice(i + 1, DeviceCollection[i], Toolkit.IsDefault(DeviceCollection[i].ID), Toolkit.IsDefaultCommunication(DeviceCollection[i].ID)));

                        //// Stop checking for other parameters
                        //return;
                    }
                }

                // Throw an exception about the received ID not being found
                throw new System.ArgumentException("No AudioDevice with that ID");
            }

            throw new System.ArgumentException("No ID provided");

        }

        static AudioDevice GetAudioDevice(int index)
        {
            // Create a new MMDeviceEnumerator
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

            //// If the Index parameter received a value
            //if (index != null)
            //{
            // Create a AudioDeviceCreationToolkit
            AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

            MMDeviceCollection DeviceCollection = null;
            try
            {
                // Enumerate all enabled devices in a collection
                DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
            }
            catch
            {
                // Error
                throw new System.Exception("Error in parameter Index - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
            }

            // If the Index is valid
            if (index >= 1 && index <= DeviceCollection.Count)
            {
                // Use valid Index as iterative
                int i = index - 1;

                // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
                return (new AudioDevice(i + 1, DeviceCollection[i], Toolkit.IsDefault(DeviceCollection[i].ID), Toolkit.IsDefaultCommunication(DeviceCollection[i].ID)));

                // Stop checking for other parameters
                //return;
            }

            // Throw an exception about the received Index not being found
            throw new System.ArgumentException("No AudioDevice with that Index");

            //}

            //// If the PlaybackCommunication switch parameter was called
            //if (playbackcommunication)
            //{
            //    // Create a AudioDeviceCreationToolkit
            //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default communication playback device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
            //    }

            //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
            //    yield return (new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

            //    // Stop checking for other parameters
            //    // return;
            //}

            //// If the PlaybackCommunicationMute switch parameter was called
            //if (playbackcommunicationmute)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default communication playback device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
            //    }

            //    // Output the mute state of the default communication playback device
            //    yield return (Device.AudioEndpointVolume.Mute);

            //    // Stop checking for other parameters
            //    //return;
            //}

            //// If the PlaybackCommunicationVolume switch parameter was called
            //if (playbackcommunicationvolume)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default communication playback device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
            //    }

            //    // Output the current volume level of the default communication playback device
            //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the Playback switch parameter was called
            //if (playback)
            //{
            //    // Create a AudioDeviceCreationToolkit
            //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default playback device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
            //    }

            //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
            //    WriteObject(new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the PlaybackMute switch parameter was called
            //if (playbackmute)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default playback device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
            //    }

            //    // Output the mute state of the default playback device
            //    WriteObject(Device.AudioEndpointVolume.Mute);

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the PlaybackVolume switch parameter was called
            //if (playbackvolume)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default playback device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
            //    }

            //    // Output the current volume level of the default playback device
            //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the RecordingCommunication switch parameter was called
            //if (recordingcommunication)
            //{
            //    // Create a AudioDeviceCreationToolkit
            //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default communication recording device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
            //    }

            //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
            //    WriteObject(new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the RecordingCommunicationMute switch parameter was called
            //if (recordingcommunicationmute)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default communication recording device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
            //    }

            //    // Output the mute state of the default communication recording device
            //    WriteObject(Device.AudioEndpointVolume.Mute);

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the RecordingCommunicationVolume switch parameter was called
            //if (recordingcommunicationvolume)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default communication recording device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
            //    }

            //    // Output the current volume level of the default communication recording device
            //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the Recording switch parameter was called
            //if (recording)
            //{
            //    // Create a AudioDeviceCreationToolkit
            //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default recording device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
            //    }

            //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
            //    WriteObject(new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the RecordingMute switch parameter was called
            //if (recordingmute)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default recording device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
            //    }

            //    // Output the mute state of the default recording device
            //    WriteObject(Device.AudioEndpointVolume.Mute);

            //    // Stop checking for other parameters
            //    return;
            //}

            //// If the RecordingVolume switch parameter was called
            //if (recordingvolume)
            //{
            //    MMDevice Device = null;
            //    try
            //    {
            //        // Get the default recording device
            //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia);
            //    }
            //    catch
            //    {
            //        // Throw an exception about the device not being found
            //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
            //    }

            //    // Output the current volume level of the default recording device
            //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

            //    // Stop checking for other parameters
            //    return;
            //}
        }

        static void SetDefaultAudioDevice(string id)
        {
            //if (defaultOnly.ToBool() && communicationOnly.ToBool())
            //    throw new System.ArgumentException("Impossible to do both DefaultOnly and CommunicatioOnly at the same time.");

            //// Create a new MMDeviceEnumerator
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

            // If the InputObject parameter received a value

            MMDeviceCollection DeviceCollection = null;
            try
            {
                // Enumerate all enabled devices in a collection
                DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
            }
            catch
            {
                // Error
                throw new System.Exception("Error in parameter InputObject - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
            }

            // For every MMDevice in DeviceCollection
            for (int i = 0; i < DeviceCollection.Count; i++)
            {
                // If this MMDevice's ID is the same as the ID of the MMDevice received by the InputObject parameter
                if (DeviceCollection[i].ID == id)
                {
                    // To use during creation of corresponding AudioDevice, assuming it is impossible to do both DefaultOnly and CommunicatioOnly at the same time
                    bool DefaultState;
                    bool CommunicationState;

                    // Create a new audio PolicyConfigClient
                    PolicyConfigClient client = new PolicyConfigClient();

                    // Create a AudioDeviceCreationToolkit
                    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

                    // Set default communication state to use
                    CommunicationState = Toolkit.IsDefaultCommunication(DeviceCollection[i].ID);

                    // Unless the CommunicationOnly parameter was called
                    //if (!communicationOnly.ToBool())
                    //{
                    // The CommunicationOnly parameter was not called

                    // Using PolicyConfigClient, set the given device as the default device (for its type)
                    client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eMultimedia);

                    // Set default state to use
                    DefaultState = true;
                    //}
                    //else
                    //{
                    //    // The CommunicationOnly parameter was called

                    //    // Set default state to use
                    //    DefaultState = Toolkit.IsDefault(DeviceCollection[i].ID);
                    //}
                }
            }
        }

        static void SetCommunicationAudioDevice(string id)
        {
            //if (defaultOnly.ToBool() && communicationOnly.ToBool())
            //    throw new System.ArgumentException("Impossible to do both DefaultOnly and CommunicatioOnly at the same time.");

            //// Create a new MMDeviceEnumerator
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

            // If the InputObject parameter received a value

            MMDeviceCollection DeviceCollection = null;
            try
            {
                // Enumerate all enabled devices in a collection
                DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
            }
            catch
            {
                // Error
                throw new System.Exception("Error in parameter InputObject - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
            }

            // For every MMDevice in DeviceCollection
            for (int i = 0; i < DeviceCollection.Count; i++)
            {
                // If this MMDevice's ID is the same as the ID of the MMDevice received by the InputObject parameter
                if (DeviceCollection[i].ID == id)
                {
                    // To use during creation of corresponding AudioDevice, assuming it is impossible to do both DefaultOnly and CommunicatioOnly at the same time
                    bool DefaultState;
                    bool CommunicationState;

                    // Create a new audio PolicyConfigClient
                    PolicyConfigClient client = new PolicyConfigClient();

                    // Create a AudioDeviceCreationToolkit
                    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

                    // Unless the DefaultOnly parameter was called
                    //if (!defaultOnly.ToBool())
                    //{
                    // The DefaultOnly parameter was not called

                    // Using PolicyConfigClient, set the given device as the default communication device (for its type)
                    client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eCommunications);

                    // Set default communication state to use
                    CommunicationState = true;
                    //}
                    //else
                    //{
                    //    // The DefaultOnly parameter was called

                    //    // Set default communication state to use
                    //    CommunicationState = Toolkit.IsDefaultCommunication(DeviceCollection[i].ID);
                    //}

                    // Unless the CommunicationOnly parameter was called
                    //if (!communicationOnly.ToBool())
                    //{
                    // The CommunicationOnly parameter was not called

                    // Using PolicyConfigClient, set the given device as the default device (for its type)
                    //client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eMultimedia);

                    // Set default state to use
                    //DefaultState = true;
                    //}
                    //else
                    //{
                    //    // The CommunicationOnly parameter was called

                    //    // Set default state to use
                    DefaultState = Toolkit.IsDefault(DeviceCollection[i].ID);
                    //}
                }
            }

        }


        // static AudioDevice GetAudioDevice(int index)
        // {
        //      // Create a new MMDeviceEnumerator
        //     MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();



        //// If the PlaybackCommunication switch parameter was called
        //if (playbackcommunication)
        //{
        //    // Create a AudioDeviceCreationToolkit
        //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default communication playback device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
        //    }

        //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
        //    yield return (new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

        //    // Stop checking for other parameters
        //    // return;
        //}

        //// If the PlaybackCommunicationMute switch parameter was called
        //if (playbackcommunicationmute)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default communication playback device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
        //    }

        //    // Output the mute state of the default communication playback device
        //    yield return (Device.AudioEndpointVolume.Mute);

        //    // Stop checking for other parameters
        //    //return;
        //}

        //// If the PlaybackCommunicationVolume switch parameter was called
        //if (playbackcommunicationvolume)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default communication playback device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
        //    }

        //    // Output the current volume level of the default communication playback device
        //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the Playback switch parameter was called
        //if (playback)
        //{
        //    // Create a AudioDeviceCreationToolkit
        //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default playback device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
        //    }

        //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
        //    WriteObject(new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the PlaybackMute switch parameter was called
        //if (playbackmute)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default playback device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
        //    }

        //    // Output the mute state of the default playback device
        //    WriteObject(Device.AudioEndpointVolume.Mute);

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the PlaybackVolume switch parameter was called
        //if (playbackvolume)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default playback device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
        //    }

        //    // Output the current volume level of the default playback device
        //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the RecordingCommunication switch parameter was called
        //if (recordingcommunication)
        //{
        //    // Create a AudioDeviceCreationToolkit
        //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default communication recording device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
        //    }

        //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
        //    WriteObject(new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the RecordingCommunicationMute switch parameter was called
        //if (recordingcommunicationmute)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default communication recording device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
        //    }

        //    // Output the mute state of the default communication recording device
        //    WriteObject(Device.AudioEndpointVolume.Mute);

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the RecordingCommunicationVolume switch parameter was called
        //if (recordingcommunicationvolume)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default communication recording device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
        //    }

        //    // Output the current volume level of the default communication recording device
        //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the Recording switch parameter was called
        //if (recording)
        //{
        //    // Create a AudioDeviceCreationToolkit
        //    AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default recording device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
        //    }

        //    // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
        //    WriteObject(new AudioDevice(Toolkit.FindIndex(Device.ID), Device, Toolkit.IsDefault(Device.ID), Toolkit.IsDefaultCommunication(Device.ID)));

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the RecordingMute switch parameter was called
        //if (recordingmute)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default recording device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
        //    }

        //    // Output the mute state of the default recording device
        //    WriteObject(Device.AudioEndpointVolume.Mute);

        //    // Stop checking for other parameters
        //    return;
        //}

        //// If the RecordingVolume switch parameter was called
        //if (recordingvolume)
        //{
        //    MMDevice Device = null;
        //    try
        //    {
        //        // Get the default recording device
        //        Device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia);
        //    }
        //    catch
        //    {
        //        // Throw an exception about the device not being found
        //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
        //    }

        //    // Output the current volume level of the default recording device
        //    WriteObject(string.Format("{0}%", Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100));

        //    // Stop checking for other parameters
        //    return;
        //}
        //}


        //static void SetAudioDevice(string id)
        //{
        //    //if (defaultOnly.ToBool() && communicationOnly.ToBool())
        //    //    throw new System.ArgumentException("Impossible to do both DefaultOnly and CommunicatioOnly at the same time.");

        //    //// Create a new MMDeviceEnumerator
        //    MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

        //    // If the InputObject parameter received a value

        //    MMDeviceCollection DeviceCollection = null;
        //    try
        //    {
        //        // Enumerate all enabled devices in a collection
        //        DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
        //    }
        //    catch
        //    {
        //        // Error
        //        throw new System.Exception("Error in parameter InputObject - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
        //    }

        //    // For every MMDevice in DeviceCollection
        //    for (int i = 0; i < DeviceCollection.Count; i++)
        //    {
        //        // If this MMDevice's ID is the same as the ID of the MMDevice received by the InputObject parameter
        //        if (DeviceCollection[i].ID == id)
        //        {
        //            // To use during creation of corresponding AudioDevice, assuming it is impossible to do both DefaultOnly and CommunicatioOnly at the same time
        //            bool DefaultState;
        //            bool CommunicationState;

        //            // Create a new audio PolicyConfigClient
        //            PolicyConfigClient client = new PolicyConfigClient();

        //            // Create a AudioDeviceCreationToolkit
        //            AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

        //            // Unless the DefaultOnly parameter was called
        //            if (!defaultOnly.ToBool())
        //            {
        //                // The DefaultOnly parameter was not called

        //                // Using PolicyConfigClient, set the given device as the default communication device (for its type)
        //                client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eCommunications);

        //                // Set default communication state to use
        //                CommunicationState = true;
        //            }
        //            else
        //            {
        //                // The DefaultOnly parameter was called

        //                // Set default communication state to use
        //                CommunicationState = Toolkit.IsDefaultCommunication(DeviceCollection[i].ID);
        //            }

        //            // Unless the CommunicationOnly parameter was called
        //            if (!communicationOnly.ToBool())
        //            {
        //                // The CommunicationOnly parameter was not called

        //                // Using PolicyConfigClient, set the given device as the default device (for its type)
        //                client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eMultimedia);

        //                // Set default state to use
        //                DefaultState = true;
        //            }
        //            else
        //            {
        //                // The CommunicationOnly parameter was called

        //                // Set default state to use
        //                DefaultState = Toolkit.IsDefault(DeviceCollection[i].ID);
        //            }
        //        }


        //        // Throw an exception about the received device not being found
        //        throw new System.ArgumentException("No such enabled AudioDevice found");
        //    }

        //    //// If the ID parameter received a value
        //    //if (!string.IsNullOrEmpty(id))
        //    //{
        //    //    MMDeviceCollection DeviceCollection = null;
        //    //    try
        //    //    {
        //    //        // Enumerate all enabled devices in a collection
        //    //        DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Error
        //    //        throw new System.Exception("Error in parameter ID - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
        //    //    }

        //    //    // For every MMDevice in DeviceCollection
        //    //    for (int i = 0; i < DeviceCollection.Count; i++)
        //    //    {
        //    //        // If this MMDevice's ID is the same as the string received by the ID parameter
        //    //        if (string.Compare(DeviceCollection[i].ID, id, System.StringComparison.CurrentCultureIgnoreCase) == 0)
        //    //        {
        //    //            // To use during creation of corresponding AudioDevice, assuming it is impossible to do both DefaultOnly and CommunicatioOnly at the same time
        //    //            bool DefaultState;
        //    //            bool CommunicationState;

        //    //            // Create a new audio PolicyConfigClient
        //    //            PolicyConfigClient client = new PolicyConfigClient();

        //    //            // Create a AudioDeviceCreationToolkit
        //    //            AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

        //    //            // Unless the DefaultOnly parameter was called
        //    //            if (!defaultOnly.ToBool())
        //    //            {
        //    //                // The DefaultOnly parameter was not called

        //    //                // Using PolicyConfigClient, set the given device as the default communication device (for its type)
        //    //                client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eCommunications);

        //    //                // Set default communication state to use
        //    //                CommunicationState = true;
        //    //            }
        //    //            else
        //    //            {
        //    //                // The DefaultOnly parameter was called

        //    //                // Set default communication state to use
        //    //                CommunicationState = Toolkit.IsDefaultCommunication(DeviceCollection[i].ID);
        //    //            }

        //    //            // Unless the CommunicationOnly parameter was called
        //    //            if (!communicationOnly.ToBool())
        //    //            {
        //    //                // The CommunicationOnly parameter was not called

        //    //                // Using PolicyConfigClient, set the given device as the default device (for its type)
        //    //                client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eMultimedia);

        //    //                // Set default state to use
        //    //                DefaultState = true;
        //    //            }
        //    //            else
        //    //            {
        //    //                // The CommunicationOnly parameter was called

        //    //                // Set default state to use
        //    //                DefaultState = Toolkit.IsDefault(DeviceCollection[i].ID);
        //    //            }

        //    //            // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
        //    //            WriteObject(new AudioDevice(i + 1, DeviceCollection[i], DefaultState, CommunicationState));

        //    //            // Stop checking for other parameters
        //    //            return;
        //    //        }
        //    //    }

        //    //    // Throw an exception about the received ID not being found
        //    //    throw new System.ArgumentException("No enabled AudioDevice found with that ID");
        //    //}

        //    //// If the Index parameter received a value
        //    //if (index != null)
        //    //{
        //    //    MMDeviceCollection DeviceCollection = null;
        //    //    try
        //    //    {
        //    //        // Enumerate all enabled devices in a collection
        //    //        DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Error
        //    //        throw new System.Exception("Error in parameter Index - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
        //    //    }

        //    //    // If the Index is valid
        //    //    if (index.Value >= 1 && index.Value <= DeviceCollection.Count)
        //    //    {
        //    //        // Use valid Index as iterative
        //    //        int i = index.Value - 1;

        //    //        // To use during creation of corresponding AudioDevice, assuming it is impossible to do both DefaultOnly and CommunicatioOnly at the same time
        //    //        bool DefaultState;
        //    //        bool CommunicationState;

        //    //        // Create a new audio PolicyConfigClient
        //    //        PolicyConfigClient client = new PolicyConfigClient();

        //    //        // Create a AudioDeviceCreationToolkit
        //    //        AudioDeviceCreationToolkit Toolkit = new AudioDeviceCreationToolkit(DevEnum);

        //    //        // Unless the DefaultOnly parameter was called
        //    //        if (!defaultOnly.ToBool())
        //    //        {
        //    //            // The DefaultOnly parameter was not called

        //    //            // Using PolicyConfigClient, set the given device as the default communication device (for its type)
        //    //            client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eCommunications);

        //    //            // Set default communication state to use
        //    //            CommunicationState = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            // The DefaultOnly parameter was called

        //    //            // Set default communication state to use
        //    //            CommunicationState = Toolkit.IsDefaultCommunication(DeviceCollection[i].ID);
        //    //        }

        //    //        // Unless the CommunicationOnly parameter was called
        //    //        if (!communicationOnly.ToBool())
        //    //        {
        //    //            // The CommunicationOnly parameter was not called

        //    //            // Using PolicyConfigClient, set the given device as the default device (for its type)
        //    //            client.SetDefaultEndpoint(DeviceCollection[i].ID, ERole.eMultimedia);

        //    //            // Set default state to use
        //    //            DefaultState = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            // The CommunicationOnly parameter was called

        //    //            // Set default state to use
        //    //            DefaultState = Toolkit.IsDefault(DeviceCollection[i].ID);
        //    //        }

        //    //        // Output the result of the creation of a new AudioDevice, while assining it its index, the MMDevice itself, its default state, and its default communication state
        //    //        WriteObject(new AudioDevice(i + 1, DeviceCollection[i], DefaultState, CommunicationState));

        //    //        // Stop checking for other parameters
        //    //        return;
        //    //    }
        //    //    else
        //    //    {
        //    //        // Throw an exception about the received Index not being found
        //    //        throw new System.ArgumentException("No enabled AudioDevice found with that Index");
        //    //    }
        //    //}

        //    //// If the PlaybackCommunicationMute parameter received a value
        //    //if (playbackcommunicationmute != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the mute state of the default communication playback device to that of the boolean value received by the Cmdlet
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications).AudioEndpointVolume.Mute = (bool)playbackcommunicationmute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
        //    //    }
        //    //}

        //    //// If the PlaybackCommunicationMuteToggle parameter was called
        //    //if (playbackcommunicationmutetoggle)
        //    //{
        //    //    try
        //    //    {
        //    //        // Toggle the mute state of the default communication playback device
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications).AudioEndpointVolume.Mute = !DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications).AudioEndpointVolume.Mute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
        //    //    }
        //    //}

        //    //// If the PlaybackCommunicationVolume parameter received a value
        //    //if (playbackcommunicationvolume != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the volume level of the default communication playback device to that of the float value received by the PlaybackCommunicationVolume parameter
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications).AudioEndpointVolume.MasterVolumeLevelScalar = (float)playbackcommunicationvolume / 100.0f;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No playback AudioDevice found with the default communication role");
        //    //    }
        //    //}

        //    //// If the PlaybackMute parameter received a value
        //    //if (playbackmute != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the mute state of the default playback device to that of the boolean value received by the Cmdlet
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia).AudioEndpointVolume.Mute = (bool)playbackmute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
        //    //    }
        //    //}

        //    //// If the PlaybackMuteToggle parameter was called
        //    //if (playbackmutetoggle)
        //    //{
        //    //    try
        //    //    {
        //    //        // Toggle the mute state of the default playback device
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia).AudioEndpointVolume.Mute = !DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia).AudioEndpointVolume.Mute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
        //    //    }
        //    //}

        //    //// If the PlaybackVolume parameter received a value
        //    //if (playbackvolume != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the volume level of the default playback device to that of the float value received by the PlaybackVolume parameter
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia).AudioEndpointVolume.MasterVolumeLevelScalar = (float)playbackvolume / 100.0f;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No playback AudioDevice found with the default role");
        //    //    }
        //    //}

        //    //// If the RecordingCommunicationMute parameter received a value
        //    //if (recordingcommunicationmute != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the mute state of the default communication recording device to that of the boolean value received by the Cmdlet
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications).AudioEndpointVolume.Mute = (bool)recordingcommunicationmute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
        //    //    }
        //    //}

        //    //// If the RecordingCommunicationMuteToggle parameter was called
        //    //if (recordingcommunicationmutetoggle)
        //    //{
        //    //    try
        //    //    {
        //    //        // Toggle the mute state of the default communication recording device
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications).AudioEndpointVolume.Mute = !DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications).AudioEndpointVolume.Mute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
        //    //    }
        //    //}

        //    //// If the RecordingCommunicationVolume parameter received a value
        //    //if (recordingcommunicationvolume != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the volume level of the default communication recording device to that of the float value received by the RecordingCommunicationVolume parameter
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications).AudioEndpointVolume.MasterVolumeLevelScalar = (float)recordingcommunicationvolume / 100.0f;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No recording AudioDevice found with the default communication role");
        //    //    }
        //    //}

        //    //// If the RecordingMute parameter received a value
        //    //if (recordingmute != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the mute state of the default recording device to that of the boolean value received by the Cmdlet
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia).AudioEndpointVolume.Mute = (bool)recordingmute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
        //    //    }
        //    //}

        //    //// If the RecordingMuteToggle parameter was called
        //    //if (recordingmutetoggle)
        //    //{
        //    //    try
        //    //    {
        //    //        // Toggle the mute state of the default recording device
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia).AudioEndpointVolume.Mute = !DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia).AudioEndpointVolume.Mute;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
        //    //    }
        //    //}

        //    //// If the RecordingVolume parameter received a value
        //    //if (recordingvolume != null)
        //    //{
        //    //    try
        //    //    {
        //    //        // Set the volume level of the default recording device to that of the float value received by the RecordingVolume parameter
        //    //        DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia).AudioEndpointVolume.MasterVolumeLevelScalar = (float)recordingvolume / 100.0f;
        //    //    }
        //    //    catch
        //    //    {
        //    //        // Throw an exception about the device not being found
        //    //        throw new System.ArgumentException("No recording AudioDevice found with the default role");
        //    //    }
        //    //}
        //}

    }
}

