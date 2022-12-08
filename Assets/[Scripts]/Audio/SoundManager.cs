using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource FXChanel;
    public AudioSource MusicChanel;
    public List<AudioClip> audioClips;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            audioClips = new List<AudioClip>();
            InitializeSoundFX();
        }
    }

    private void InitializeSoundFX()
    {
        audioClips.Add(Resources.Load<AudioClip>("Audio/Jump-Sound"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/Hurt-Sound"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/Lose-Sound"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/MainTheme"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/EndTheme"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/Fire-Sound"));
    }

    public void PlaySoundFX(Sound sound, float relativeVolume)
    {
        FXChanel.PlayOneShot(audioClips[(int)sound]);
    }

    public void PlayMusic(Sound music, float volume, bool loop)
    {
        MusicChanel.clip = audioClips[(int)music];
        MusicChanel.volume = volume;
        MusicChanel.loop = loop;
        MusicChanel.Play();
    }
}
