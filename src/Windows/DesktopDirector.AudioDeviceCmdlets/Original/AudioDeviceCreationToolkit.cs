using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAudioApi
{ 
    public class AudioDeviceCreationToolkit
    {
        // The MMDeviceEnumerator
        public MMDeviceEnumerator DevEnum;

        // To be created, a new AudioDeviceCreationToolkit needs a MMDeviceEnumerator it will use to compare the ID its methods receive
        public AudioDeviceCreationToolkit(MMDeviceEnumerator DevEnum)
        {
            // Set this object's DeviceEnumerator to the received MMDeviceEnumerator
            this.DevEnum = DevEnum;
        }

        // Method to find out, in a collection of all enabled MMDevice, the Index of a MMDevice, given its ID
        public int FindIndex(string ID)
        {
            MMDeviceCollection DeviceCollection = null;
            try
            {
                // Enumerate all enabled devices in a collection
                DeviceCollection = DevEnum.EnumerateAudioEndPoints(EDataFlow.eAll, EDeviceState.DEVICE_STATE_ACTIVE);
            }
            catch
            {
                // Error
                throw new System.Exception("Error in method AudioDeviceCreationToolkit.FindIndex(string ID) - Failed to create the collection of all enabled MMDevice using MMDeviceEnumerator");
            }

            // For each device in the collection
            for (int i = 0; i < DeviceCollection.Count; i++)
            {
                // If the received ID is the same as this device's ID
                if (DeviceCollection[i].ID == ID)
                {
                    // Return this device's Index
                    return (i + 1);
                }
            }

            // Error
            throw new System.Exception("Error in method AudioDeviceCreationToolkit.FindIndex(string ID) - No MMDevice with the given ID was found in the collection of all enabled MMDevice");
        }

        // Method to find out if a MMDevice is the default MMDevice of its type, given its ID
        public bool IsDefault(string ID)
        {
            string PlaybackID = "";
            try
            {
                // Get the ID of the default playback device
                PlaybackID = (DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia)).ID;
            }
            catch { }

            // If the received ID is the same as the default playback device's ID
            if (ID == PlaybackID)
            {
                return (true);
            }

            string RecordingID = "";
            try
            {
                // Get the ID of the default recording device
                RecordingID = (DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia)).ID;
            }
            catch { }

            // If the received ID is the same as the default recording device's ID
            if (ID == RecordingID)
            {
                return (true);
            }

            return (false);
        }

        // Method to find out if a MMDevice is the default communication MMDevice of its type, given its ID
        public bool IsDefaultCommunication(string ID)
        {
            string PlaybackCommunicationID = "";
            try
            {
                // Get the ID of the default communication playback device
                PlaybackCommunicationID = (DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eCommunications)).ID;
            }
            catch { }

            // If the received ID is the same as the default communication playback device's ID
            if (ID == PlaybackCommunicationID)
            {
                return (true);
            }

            string RecordingCommunicationID = "";
            try
            {
                // Get the ID of the default communication recording device
                RecordingCommunicationID = (DevEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications)).ID;
            }
            catch { }

            // If the received ID is the same as the default communication recording device's ID
            if (ID == RecordingCommunicationID)
            {
                return (true);
            }

            return (false);
        }
    }
}
