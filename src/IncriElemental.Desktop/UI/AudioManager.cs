using Microsoft.Xna.Framework.Audio;

namespace IncriElemental.Desktop.UI;

public class AudioManager
{
    private DynamicSoundEffectInstance _instance;
    private DynamicSoundEffectInstance _humInstance;
    private const int SampleRate = 44100;
    private const int Channels = 1;
    private float _phase = 0;
    private float _humPhase1 = 0;
    private float _humPhase2 = 0;

    public AudioManager()
    {
        _instance = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
        _humInstance = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Mono);
        _humInstance.BufferNeeded += (s, e) => FillHumBuffer();
    }

    public void StartHum()
    {
        if (_humInstance.State != SoundState.Playing)
        {
            FillHumBuffer();
            FillHumBuffer();
            _humInstance.Play();
        }
    }

    private void FillHumBuffer()
    {
        const int samples = 4410; // 100ms
        var buffer = new byte[samples * 2];
        const float vol = 0.005f;

        for (var i = 0; i < samples; i++)
        {
            var s1 = Math.Sin(_humPhase1);
            var s2 = Math.Sin(_humPhase2);
            var sample = (short)((s1 + s2) * 0.5f * vol * short.MaxValue);
            
            buffer[i * 2] = (byte)(sample & 0xFF);
            buffer[i * 2 + 1] = (byte)(sample >> 8);

            _humPhase1 += (float)(2 * Math.PI * 50.0 / SampleRate);
            _humPhase2 += (float)(2 * Math.PI * 52.0 / SampleRate);
            if (_humPhase1 > 2 * Math.PI) _humPhase1 -= (float)(2 * Math.PI);
            if (_humPhase2 > 2 * Math.PI) _humPhase2 -= (float)(2 * Math.PI);
        }
        _humInstance.SubmitBuffer(buffer);
    }

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
