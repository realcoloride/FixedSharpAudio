using System;
using Vortice.Multimedia;
using Vortice.XAudio2;

namespace SharpAudio.XA2
{
    internal sealed class XA2Source : AudioSource
    {
        private readonly XA2Engine _engine;
        private readonly XA2Submixer _submixer;

        internal IXAudio2SourceVoice _sourceVoice = null;


        public IXAudio2SourceVoice SourceVoice {
            get
            {
                if (_sourceVoice == null)
                    SetupVoice();

                return _sourceVoice;
            }
        }

        public XA2Source(XA2Engine engine, XA2Submixer submixer)
        {
            _engine = engine;
            _submixer = submixer;
        }

        private void SetupVoice()
        {
            if (Format == null) throw new Exception("No format for this source has been set up. Please set up the format using the Format property.");

            AudioFormat format = Format.Value;

            var wFmt = new WaveFormat(format.SampleRate, format.BitsPerSample, format.Channels);
            _sourceVoice = _engine.Device.CreateSourceVoice(wFmt);

            if (_submixer != null)
            {
                var vsDesc = new VoiceSendDescriptor { OutputVoice = _submixer.SubMixerVoice };
                _sourceVoice.SetOutputVoices(new VoiceSendDescriptor[] { vsDesc });
            }

            _sourceVoice.SetVolume(_volume);
        }

        public override int BuffersQueued => SourceVoice?.State.BuffersQueued ?? 0;

        public override float Volume
        {
            get => _volume;
            set { _volume = value; SourceVoice?.SetVolume(value); }
        }

        public override bool Looping
        {
            get => _looping;
            set { _looping = value; }
        }

        public override void Dispose()
        {
            SourceVoice?.DestroyVoice();
            SourceVoice?.Dispose();
        }

        public override bool IsPlaying() => BuffersQueued > 0;

        public override void Play() => SourceVoice?.Start();

        public override void Stop() => SourceVoice?.Stop();

        public override void QueueBuffer(AudioBuffer buffer)
        {
            var xaBuffer = (XA2Buffer) buffer;
            if (_looping) xaBuffer.Buffer.LoopCount = IXAudio2.LoopInfinite;

            SourceVoice.SubmitSourceBuffer(xaBuffer.Buffer, null);
        }

        public override void Flush() => SourceVoice.FlushSourceBuffers();
    }
}
