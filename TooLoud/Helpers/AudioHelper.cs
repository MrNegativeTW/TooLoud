﻿using NAudio.CoreAudioApi;
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

        private int mainMaximunVolumn;

        #endregion

        public AudioHelper() {
            Initialize();
        }

        public void Initialize() {
            mainMaximunVolumn = AppDataHelper.MainMaximunVolumn;

            client = new AudioDeviceNotificationClient();

            enumerator = new MMDeviceEnumerator();
            enumerator.RegisterEndpointNotificationCallback(client);

            if (enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia)) {
                UpdateDevice(enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia));
            }

            OnEnabled();
        }


        protected override void OnEnabled() {
            base.OnEnabled();
            if (IsEnabled == false) { return; }

            client.DefaultDeviceChanged += Client_DefaultDeviceChanged;

            if (device != null) {
                device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                //PrimaryContent = volumeControl;
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
            Trace.WriteLine($"Curent volume is {volume}");

            if (volume > mainMaximunVolumn) {
                Trace.WriteLine($"{device.AudioEndpointVolume.MasterVolumeLevelScalar}, {(mainMaximunVolumn / 100f)}");
                if (device.AudioEndpointVolume.MasterVolumeLevelScalar > (mainMaximunVolumn / 100f)) {
                    device.AudioEndpointVolume.MasterVolumeLevelScalar = mainMaximunVolumn / 100f;
                }
            }
            //Application.Current.Dispatcher.Invoke(() => {
            //    UpdateVolumeGlyph(volume);
            //    volumeControl.textVal.Text = Math.Round(volume).ToString("00");
            //    _isInCodeValueChange = true;
            //    volumeControl.VolumeSlider.Value = volume;
            //    _isInCodeValueChange = false;
            //});
        }
    }
}
