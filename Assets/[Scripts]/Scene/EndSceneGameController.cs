using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneGameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayMusic(Sound.ENDMUSIC, 0.25f, true);
    }
}
