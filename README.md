# FixedSharpAudio

FixedSharpAudio is a fork of SharpAudio, a cross-platform, backend-agnostic library to playback sounds in .NET. It achieves that by wrapping the platform specific backends.

The fork is meant to be more stable and easier to use.

Supported backends:
- XAudio2
- OpenAL

# Example

SharpAudio provides a low-level interface that wraps audio sources & buffers:
```csharp
    // Create the engine depending on the OS (OpenAL/XAudio2)
    AudioEngine audioEngine = AudioEngine.CreateDefault();

    // Create the buffer to allow data from coming in
    AudioBuffer audioBuffer = audioEngine.CreateBuffer();

    // Create the source for the playback
    AudioSource audioSource = audioEngine.CreateSource();

    // Play a 1s long sound at 440hz
    AudioFormat format;
    format.BitsPerSample = 16;
    format.Channels = 1;
    format.SampleRate = 44100;

    // Set the format for playback if using XAudio2
    audioSource.Format = format;

    // Generate procedural audio at 440 Hz
    float freq = 440.0f;
    var size = format.SampleRate;
    var samples = new short[size];

    for (int i = 0; i < size; i++)
    {
        samples[i] = (short)(32760 * Math.Sin((2 * Math.PI * freq) / size * i));
    }

    // Buffer the data up and queue it for playback
    audioBuffer.BufferData(samples, format);
    audioSource.QueueBuffer(buffer);

    // Play the data
    audioSource.Play();
```

A high level interface that can load and play sound files is provided in the SharpAudio.Codec package:
```csharp
    // Create the engine depending on the OS (OpenAL/XAudio2)
    AudioEngine audioEngine = AudioEngine.CreateDefault();
    SoundStream soundStream = new(File.OpenRead("test.mp3"), audioEngine);

    soundStream.Volume = 0.5f;
    soundStream.Play();
```

The following sound formats are supported at the moment:
- `.wav` (PCM & ADPCM)
- `.mp3` 
- `.ogg` (WIP: Vorbis & Opus)
