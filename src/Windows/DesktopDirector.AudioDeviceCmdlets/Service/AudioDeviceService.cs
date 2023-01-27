using CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDirector.AudioDeviceCmdlets.Service
{
    /// <summary>
    /// This service is a port of the original AudioDeviceCmdlets.cs from https://github.com/frgnca/AudioDeviceCmdlets/tree/master/SOURCE
    /// The main converion is to simplify the calls from an application
    /// </summary>
    public class AudioDeviceService : IAudioDeviceService
    {
        public IList<AudioDevice> GetAudioDevices()
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
        public AudioDevice GetAudioDevice(string id)
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

        public AudioDevice GetAudioDevice(int index)
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
        }

        public void SetDefaultAudioDevice(string id)
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
        public void SetCommunicationAudioDevice(string id)
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
    }
}
