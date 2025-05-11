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

            // Get and set the initial default device.
            // UpdateDevice will handle if defaultDevice is null and respect the IsEnabled state.
            MMDevice defaultDevice = null;
            if (enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia)) {
                try {
                    defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                } catch (Exception ex) {
                    Trace.WriteLine($"Error getting default audio endpoint on Initialize: {ex.Message}");
                    // defaultDevice remains null, UpdateDevice will handle this
                }
            }
            UpdateDevice(defaultDevice);
        }


        protected override void OnEnabled() {
            base.OnEnabled();
            if (IsEnabled == false) { return; }

            client.DefaultDeviceChanged += Client_DefaultDeviceChanged;

            if (device != null) {
                try {
                    // Defensive unsubscription to prevent duplicates if logic paths overlap.
                    device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
                    device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                    UpdateVolume(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100); // Apply limit
                } catch (Exception ex) {
                    Trace.WriteLine($"Error subscribing/updating volume for device '{device.FriendlyName}' on OnEnabled: {ex.Message}");
                }
            } else {
                // If no device is currently set, try to get the current default one.
                // This can happen if the app starts disabled, then is enabled, and no device change event has fired yet.
                MMDevice currentDefaultDevice = null;
                if (enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia)) {
                    try {
                        currentDefaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                    } catch (Exception ex) {
                        Trace.WriteLine($"Error getting default audio endpoint on OnEnabled: {ex.Message}");
                    }
                }
                UpdateDevice(currentDefaultDevice); // This will subscribe if a device is found and IsEnabled is true.
            }
        }

        protected override void OnDisabled() {
            base.OnDisabled();

            // Unsubscribe from default device changes.
            client.DefaultDeviceChanged -= Client_DefaultDeviceChanged;

            if (device != null) {
                try {
                    device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
                } catch (Exception ex) {
                    Trace.WriteLine($"Error unsubscribing volume notifications for device '{device.FriendlyName}' on OnDisabled: {ex.Message}");
                }
            }
        }

        private void Client_DefaultDeviceChanged(object sender, string deviceId) {
            // MMDevice mmdevice = string.IsNullOrEmpty(e) ? null : enumerator.GetDevice(e);
            // UpdateDevice(mmdevice);
            MMDevice newDevice = null;
            if (!string.IsNullOrEmpty(deviceId)) {
                try {
                    newDevice = enumerator.GetDevice(deviceId);
                } catch (Exception ex) {
                    Trace.WriteLine($"Error getting device for ID '{deviceId}': {ex.Message}");
                    // newDevice remains null
                }
            }
            UpdateDevice(newDevice);
        }

        // Generated by Gemini 2.5 Pro
        private void UpdateDevice(MMDevice newDevice) {
            string oldDeviceName = device?.FriendlyName ?? "null";
            string newDeviceName = newDevice?.FriendlyName ?? "null";
            Trace.WriteLine($"UpdateDevice: Old='{oldDeviceName}', New='{newDeviceName}'");
            
            // Unsubscribe from the old device if it exists
            if (device != null) {
                Trace.WriteLine($"Unsubscribing {device.FriendlyName}");
                try {
                    device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
                } catch (Exception ex) {
                    // Log error, but continue to set the new device.
                    Trace.WriteLine($"  > Error unsubscribing from old device {device.FriendlyName}: {ex.Message}");
                }
                // Consider if device needs to be disposed: device.Dispose(); (Check NAudio docs for MMDevice lifecycle)
            }

            device = newDevice; // Assign the new device

            if (device != null) {
                Trace.WriteLine($"New device: {device.FriendlyName}");
                if (IsEnabled) { // Only subscribe and update volume if the helper is currently enabled
                    Trace.WriteLine($"Subscribing...");
                    try {
                        // Defensive unsubscription before adding, just in case.
                        device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
                        device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                        UpdateVolume(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                    } catch (Exception ex) {
                        Trace.WriteLine($"Error during new device ('{device.FriendlyName}'), {ex.Message}");
                        // This might happen if the device is in a strange state (e.g., unplugged just as we access it)
                    }
                } else {
                    Trace.WriteLine($"IsEnabled is false");
                }
            } else {
                Trace.WriteLine("New device is null.");
            }
        }
          
        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data) {
            if (!IsEnabled) { // Safeguard: Should not receive notifications if disabled
                Trace.WriteLine($"OnVolumeNotification called but IsEnabled is false, Ignoring.");
                return;
            }
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
