using System;
using CSCore;
using CSCore.DSP;
using CSCore.Utils;

namespace Visualisation
{
    public class SampleAnalyser
    {
        private bool _isInitialized;
        private int _channels;
        private readonly Complex[] _storedSamples;
        private int _sampleOffset;

        public SampleAnalyser(int storageSize)
        {
            _storedSamples = new Complex[storageSize];
        }

        public void Initialize(int channels)
        {
            _channels = channels;

            if (_isInitialized)
                throw new InvalidOperationException("Can't reuse SampleAnalyser.");

            _isInitialized = true;
        }

        public void Add(float left, float right)
        {
            var arr = _channels == 1 ? new[] { left } : new[] { left, right };
            Add(arr);
        }

        public void Add(float[] samples)
        {
            if (!_isInitialized) return;

            if (samples.Length % _channels != 0)
                throw new ArgumentException("Length of samples to add has to be multiple of the channelCount.");

            int i = 0;
            while (i < samples.Length)
            {
                float s = MergeSamples(samples, i, _channels);
                _storedSamples[_sampleOffset].Real = s;
                _storedSamples[_sampleOffset].Imaginary = 0f;

                _sampleOffset += 1;

                if (_sampleOffset >= _storedSamples.Length)
                {
                    _sampleOffset = 0;
                }
                i += _channels;
            }
        }

        // ReSharper disable once InconsistentNaming
        public void CalculateFFT(float[] resultBuffer)
        {
            Complex[] input = new Complex[_storedSamples.Length];
            _storedSamples.CopyTo(input, 0);

            FastFourierTransformation.Fft(input, Convert.ToInt32(Math.Truncate(Math.Log(_storedSamples.Length, 2))));
            for (int i = 0; i <= input.Length / 2 - 1; i++)
            {
                var z = input[i];
                resultBuffer[i] = (float)z.Value;
            }
        }

        private float MergeSamples(float[] samples, int index, int channelCount)
        {
            var z = 0f;
            for (int i = 0; i < channelCount; i++)
            {
                z += samples[index + i];
            }
            return z / channelCount;
        }
    }
}
