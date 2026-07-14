using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class AudioService
{
    private AudioManager audioManager;

    public AudioService(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }



    public void PlaySFX(SoundTypes soundToUsed)
    {
        if (audioManager == null)
        {
            Console.WriteLine("Audio Manager is null");
            return;
        }
        audioManager.PlaySFXSounds(soundToUsed);
    }

    public void Stopbgm()
    {
        audioManager.StopBgm();
    }
}

