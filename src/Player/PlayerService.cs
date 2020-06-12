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
        private readonly IApplicationStateService _stateService;
        private MMDevice _device;
        private ISoundOut _soundOut;
        private float _volume;
        private readonly Timer _timerFinishedTrack;

        public PlayerService(IApplicationStateService stateService)
        {
            _stateService = stateService;
            _timerFinishedTrack = new Timer(CallbackFinishedTrack);

            Observable.Timer(DateTimeOffset.Now, TimeSpan.FromMilliseconds(500))
                .Where(_ => _soundOut?.WaveSource != null)
                .Select(_ => _soundOut.WaveSource.GetPosition())
                .Subscribe(stateService.ActualAvancement);

            stateService.PlayingTrack
                .Select(t => t?.Resource)
                .DistinctUntilChanged()
                .Subscribe(async uri => await Play(uri));

            stateService.PlayerState.Select(p => p.PlayingState)
                .DistinctUntilChanged()
                .Subscribe(PauseResume);


            stateService.PlayerState.Select(p => p.VolumentPercentage)
                .DistinctUntilChanged()
                .Select(v => v / 100f)
                .Subscribe(ChangeVolume);
        }

        private void CallbackFinishedTrack(object state)
        {
            _timerFinishedTrack.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            var finishedIn = _soundOut.WaveSource.GetLength() - _soundOut.WaveSource.GetPosition();

            if (finishedIn > TimeSpan.FromMilliseconds(10))
            {
                _timerFinishedTrack.Change(finishedIn, Timeout.InfiniteTimeSpan);
                return;
            }

            _stateService.Next();
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

        private async Task Play(Uri file)
        {
            if (file is null)
            {
                // Nothing playing
                _stateService.ActualAvancement(TimeSpan.Zero);
                _soundOut?.Stop();
                _soundOut?.Dispose();
                return;
            }

            var waveSource = await Task.Run(() => CodecFactory.Instance
                .GetCodec(file)
                .ToSampleSource()
                .ToWaveSource());

            _soundOut?.Stop();
            _soundOut?.Dispose();

            _soundOut = new WasapiOut
            {
                Latency = 100,
                UseChannelMixingMatrices = true,
                StreamRoutingOptions = StreamRoutingOptions.OnDefaultDeviceChange
            };
            _soundOut.Initialize(waveSource);
            _soundOut.Volume = _volume;

            _timerFinishedTrack.Change(_soundOut.WaveSource.GetLength(), Timeout.InfiniteTimeSpan);
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
