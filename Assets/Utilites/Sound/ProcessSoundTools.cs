using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProcessSoundTools
{
    public static void SetSound(this AudioSource audio, float factor)
    {
        if (audio == null) return;

        if (factor == 0)
        {
            audio.StopSound();

            return;
        }

        if (!audio.isPlaying) audio.Play();

        audio.pitch = factor;

        audio.volume = factor > 1 ? 1 : factor;
    }

    public static void StopSound(this AudioSource audio)
    {
        if (audio == null) return;

        if (audio.isPlaying) audio.Stop();
    }
}
