using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sms;
using Windows.Media.Protection.PlayReady;

namespace TooLoud.Helpers {
    public class AudioHelper : TooLoudHelperBase {

        private AudioDeviceNotificationClient client;
        private MMDeviceEnumerator enumerator;
        private MMDevice device;

        #region Properties

        private int mainMaximumVolume;

        #endregion

        public AudioHelper() {
            AppSettingsHelper.ProtectionEnabledChanged += OnProtectionEnabledChanged;
            AppSettingsHelper.MainMaximumVolumeChanged += OnMainMaximumVolumeChanged;

            Initialize();

            if (IsEnabled) {
                OnEnabled();
            }
        }

        private void OnProtectionEnabledChanged(object sender, bool enabled) {
            IsEnabled = enabled;
        }

        private void OnMainMaximumVolumeChanged(object sender, int volume) {
            mainMaximumVolume = volume;
        }

        public void Initialize() {
            //mainMaximumVolume = AppDataHelper.MainMaximumVolume;
            mainMaximumVolume = AppSettingsHelper.MainMaximumVolume;

            client = new AudioDeviceNotificationClient();

            enumerator = new MMDeviceEnumerator();
            enumerator.RegisterEndpointNotificationCallback(client);

            if (enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia)) {
                UpdateDevice(enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia));
            }
        }


        protected override void OnEnabled() {
            base.OnEnabled();
            if (IsEnabled == false) { return; }

            client.DefaultDeviceChanged += Client_DefaultDeviceChanged;

            if (device != null) {
                device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                //PrimaryContent = volumeControl;

                //UpdateVolume(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
            }
        }

        protected override void OnDisabled() {
            base.OnDisabled();

            client.DefaultDeviceChanged -= Client_DefaultDeviceChanged;

            if (device != null) {
                device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
            }
        }

        private void Client_DefaultDeviceChanged(object sender, string e) {
            MMDevice mmdevice = string.IsNullOrEmpty(e) ? null : enumerator.GetDevice(e);
            UpdateDevice(mmdevice);
        }

        private void UpdateDevice(MMDevice mmdevice) {
            if (device != null) {
                device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
            }

            device = mmdevice;
            if (device != null) {
                try {
                    UpdateVolume(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                    device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                } catch {
                
                }

                //Application.Current.Dispatcher.Invoke(() => PrimaryContent = volumeControl);
            } else { 
                //Application.Current.Dispatcher.Invoke(() => PrimaryContent = noDeviceMessageBlock);
            }
        }
          
        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data) {
            UpdateVolume(data.MasterVolume * 100);
        }

        private void UpdateVolume(double volume) {
            int roundedVolume = (int) Math.Round(volume);

            Trace.WriteLine($"OnVolumeNotification: Current/Limit: {roundedVolume}/{mainMaximumVolume}");

            if (roundedVolume <= mainMaximumVolume) {
                return;
            }

            float targetVolume = mainMaximumVolume / 100f;
            float currentVolume = device.AudioEndpointVolume.MasterVolumeLevelScalar;
            if (currentVolume > targetVolume) {
                device.AudioEndpointVolume.MasterVolumeLevelScalar = targetVolume;
            }
        }
    }
}
