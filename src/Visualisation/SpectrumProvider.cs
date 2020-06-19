using System;
using System.ComponentModel;
using System.Reactive.Linq;
using ApplicationState;
using ApplicationState.States;
using CSCore.Streams;
using WPFSoundVisualizationLib;

namespace Visualisation
{
    public class SpectrumProvider : ISpectrumPlayer
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private SampleAnalyser _analyser;
        private float? _sampleRate;
        private bool _isPlaying;
        private const int FFTSize = 4096;

        public SpectrumProvider(IApplicationStateService applicationState)
        {
            applicationState.PlayerState
                .Select(p => p.Channels)
                .DistinctUntilChanged()
                .Subscribe(ChannelsChanged);

            applicationState.PlayerState
                .Select(p => p.SampleRate)
                .DistinctUntilChanged()
                .Subscribe(SampleRateChanged);

            applicationState.PlayerState
                .Select(p => p.PlayingState == PlayingStateEnum.Playing)
                .DistinctUntilChanged()
                .Subscribe(IsPlayingChanged);
        }

        public bool IsPlaying => _isPlaying;

        public void SourceSingleBlockRead(SingleBlockReadEventArgs e)
        {
            _analyser?.Add(e.Left, e.Right);
        }

        private void IsPlayingChanged(bool isPlaying)
        {
            _isPlaying = isPlaying;
        }
        private void SampleRateChanged(int sampleRate)
        {
            _sampleRate = sampleRate;
        }
        private void ChannelsChanged(int channels)
        {
            _analyser = new SampleAnalyser(FFTSize);
            _analyser.Initialize(channels);
        }


        public bool GetFFTData(float[] fftDataBuffer)
        {
            if (_analyser is null) return false;

            _analyser.CalculateFFT(fftDataBuffer);
            return IsPlaying;
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            double f;
            if (_sampleRate.HasValue)
            {
                f = _sampleRate.Value / 2.0;
            }
            else
            {
                f = 22050; //44100 / 2
            }
            return Convert.ToInt32((frequency / f) * (FFTSize / 2));
        }
    }
}
