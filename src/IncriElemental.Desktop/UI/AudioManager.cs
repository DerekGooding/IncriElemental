using System;
using Microsoft.Xna.Framework.Audio;

namespace IncriElemental.Desktop.UI;

public class AudioManager
{
    private DynamicSoundEffectInstance _instance;
    private const int SampleRate = 44100;
    private const int Channels = 1;
    private float _phase = 0;

    public AudioManager() => _instance = new DynamicSoundEffectInstance(SampleRate, Channels == 1 ? AudioChannels.Mono : AudioChannels.Stereo);

    public void PlayTone(float frequency, float duration, float volume = 0.1f)
    {
        var sampleCount = (int)(SampleRate * duration);
        var buffer = new byte[sampleCount * 2];

        for (var i = 0; i < sampleCount; i++)
        {
            var sample = (short)(Math.Sin(_phase) * volume * short.MaxValue);
            buffer[i * 2] = (byte)(sample & 0xFF);
            buffer[i * 2 + 1] = (byte)(sample >> 8);

            _phase += (float)(2 * Math.PI * frequency / SampleRate);
            if (_phase > 2 * Math.PI) _phase -= (float)(2 * Math.PI);
        }

        _instance.SubmitBuffer(buffer);
        if (_instance.State != SoundState.Playing) _instance.Play();
    }

    public void PlayFocus() => PlayTone(440, 0.05f, 0.05f);
    public void PlayManifest() => PlayTone(220, 0.2f, 0.1f);
    public void PlayExplore() => PlayTone(880, 0.1f, 0.05f);
    public void PlayAscend() => PlayTone(554.37f, 1.0f, 0.2f); // C#5
}
