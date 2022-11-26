using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioSource> channels;
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
            channels = GetComponents<AudioSource>().ToList();
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
    }

    public void PlaySoundFX(Sound sound, Channel channel)
    {
        channels[(int)channel].clip = audioClips[(int)sound];
        channels[(int)channel].Play();
    }

    public void PlayMusic(float volume, bool loop)
    {
        channels[(int)Channel.MUSIC].clip = audioClips[(int)Channel.MUSIC];
        channels[(int)Channel.MUSIC].volume = volume;
        channels[(int)Channel.MUSIC].loop = loop;
        channels[(int)Channel.MUSIC].Play();
    }
}
