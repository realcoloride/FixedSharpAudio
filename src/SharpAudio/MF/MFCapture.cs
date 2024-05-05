using Vortice.MediaFoundation;

namespace SharpAudio.MF
{
    internal sealed class MFCapture : AudioCapture
    {
        private readonly IMFMediaSource _audioSource;
        public override AudioBackend BackendType => AudioBackend.MediaFoundation;

        public MFCapture(AudioCaptureOptions _)
        {
            IMFAttributes attribs = null;
            MediaFactory.MFCreateDeviceSource(attribs, out _audioSource);
        }

        protected override void PlatformDispose()
        {
            _audioSource.Shutdown();
            _audioSource.Release();
        }
    }
}
