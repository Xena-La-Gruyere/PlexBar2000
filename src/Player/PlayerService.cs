using System;
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
using Microsoft.Extensions.Hosting;

namespace Player
{
    public class PlayerService : IHostedService
    {
        private MMDevice _device;
        private ISoundOut _soundOut;

        public PlayerService(IApplicationStateService stateService)
        {
            Observable.Timer(DateTimeOffset.Now, TimeSpan.FromMilliseconds(500))
                .Where(_ => _soundOut?.WaveSource?.Position != null)
                .Select(_ => (double)_soundOut.WaveSource.Position / (double)_soundOut.WaveSource.Length)
                .Subscribe(stateService.ActualAvancement);

            stateService.PlayerState.Select(p => p.PlayingTrack)
                .Select(t => t?.Resource)
                .Where(uri => uri != null)
                .DistinctUntilChanged()
                .Subscribe(Play);

            stateService.PlayerState.Select(p => p.PlayingState)
                .DistinctUntilChanged()
                .Subscribe(PauseResume);
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

        private void Play(Uri file)
        {
            var waveSource = CodecFactory.Instance
                .GetCodec(file)
                .ToSampleSource()
                .ToWaveSource();

            _soundOut = new WasapiOut
            {
                Latency = 100,
                UseChannelMixingMatrices = true,
                StreamRoutingOptions = StreamRoutingOptions.OnDefaultDeviceChange
            };
            _soundOut.Initialize(waveSource);
            _soundOut.Play();
        }

        private void PauseResume(PlayingStateEnum state)
        {
            if(state == PlayingStateEnum.Paused && 
               _soundOut?.PlaybackState == PlaybackState.Playing)
                _soundOut?.Pause();

            if (state == PlayingStateEnum.Playing &&
                _soundOut?.PlaybackState == PlaybackState.Paused)
                _soundOut?.Resume();
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
