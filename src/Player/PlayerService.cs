﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationState;
using ApplicationState.States;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using Microsoft.Extensions.Hosting;
using Visualisation;

namespace Player
{
    public class PlayerService : IHostedService
    {
        private SingleBlockNotificationStream _singleBlockNotificationStream;
        private readonly IApplicationStateService _stateService;
        private readonly SpectrumProvider _spectrumProvider;
        private MMDevice _device;
        private ISoundOut _soundOut;
        private float _volume;

        public PlayerService(
            IApplicationStateService stateService,
            SpectrumProvider spectrumProvider)
        {
            _stateService = stateService;
            _spectrumProvider = spectrumProvider;

            Observable.Timer(DateTimeOffset.Now, TimeSpan.FromMilliseconds(500))
                .Where(_ => _soundOut?.WaveSource != null)
                .Select(_ => _soundOut.WaveSource.GetPosition())
                .Subscribe(stateService.ActualAvancement);

            stateService.PlayingTrack
                .Select(t => t?.Resource)
                .DistinctUntilChanged()
                .Subscribe(uri => Play(uri));

            stateService.PlayerState.Select(p => p.PlayingState)
                .Where(_ => _soundOut != null)
                .DistinctUntilChanged()
                .Subscribe(StateChangedWhenInitialized);


            stateService.PlayerState.Select(p => p.VolumentPercentage)
                .DistinctUntilChanged()
                .Select(v => v / 100f)
                .Subscribe(ChangeVolume);
        }

        private void ChangeVolume(float volume)
        {
            _volume = volume;

            if (_soundOut is null) return;
            
            _soundOut.Volume = volume;
        }

        private IEnumerable<MMDevice> GetDevices()
        {
            using var mmdeviceEnumerator = new MMDeviceEnumerator();
            using var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
            foreach (var device in mmdeviceCollection)
                yield return device;
        }

        private void SetDevice(MMDevice device)
        {
            _device = device;
        }

        private void Stop()
        {
            _stateService.ActualAvancement(TimeSpan.Zero);

            if (_soundOut is null) return;

            _soundOut.Stopped -= SoundOutOnStopped;
            if (_soundOut.PlaybackState != PlaybackState.Stopped)
                _soundOut.Stop();
            _soundOut.Dispose();
        }

        private async Task Play(Uri file)
        {
            if (file is null)
            {
                // Nothing playing
                Stop();
                return;
            }

            if(_singleBlockNotificationStream != null)
                _singleBlockNotificationStream.SingleBlockRead -= notificationSource_SingleBlockRead;

            var waveSource = await Task.Run(() => CodecFactory.Instance
                .GetCodec(file)
                .ToSampleSource()
                .AppendSource(x => new SingleBlockNotificationStream(x), out _singleBlockNotificationStream)
                .ToWaveSource());


            _singleBlockNotificationStream.SingleBlockRead += notificationSource_SingleBlockRead;

            _stateService.ActualFormat(waveSource.WaveFormat.Channels, waveSource.WaveFormat.SampleRate);

            Stop();

            _soundOut = new WasapiOut
            {
                Latency = 20,
                UseChannelMixingMatrices = true,
                StreamRoutingOptions = StreamRoutingOptions.OnDefaultDeviceChange
            };
            _soundOut.Stopped += SoundOutOnStopped;
            _soundOut.Initialize(waveSource);
            _soundOut.Volume = _volume;

            _soundOut.Play();
        }

        private void notificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            _spectrumProvider.SourceSingleBlockRead(e);
        }

        private void SoundOutOnStopped(object sender, PlaybackStoppedEventArgs e)
        {
            _stateService.Next();
        }

        private void StateChangedWhenInitialized(PlayingStateEnum state)
        {
            if(state == PlayingStateEnum.Paused &&
               _soundOut.PlaybackState == PlaybackState.Playing)
                _soundOut.Pause();

            if (state == PlayingStateEnum.Playing &&
                _soundOut.PlaybackState == PlaybackState.Paused)
                _soundOut.Resume();

            if (state == PlayingStateEnum.Stopped)
                Stop();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _soundOut?.Dispose();

            return Task.CompletedTask;
        }
    }
}
