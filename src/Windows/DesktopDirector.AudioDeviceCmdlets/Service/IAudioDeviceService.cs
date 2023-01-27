using CoreAudioApi;

namespace DesktopDirector.AudioDeviceCmdlets.Service
{
    public interface IAudioDeviceService
    {
        IList<AudioDevice> GetAudioDevices();
        AudioDevice GetAudioDevice(string id);
        AudioDevice GetAudioDevice(int index);
        void SetDefaultAudioDevice(string id);
        void SetCommunicationAudioDevice(string id);
    }
}
