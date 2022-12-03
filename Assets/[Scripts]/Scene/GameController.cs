using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject OnScreenControls;

    public bool enableMusic = true;

    private void Awake()
    {
        OnScreenControls = GameObject.Find("OnScreenControls");

        OnScreenControls.SetActive(Application.isMobilePlatform);

        if(enableMusic)
            SoundManager.instance.PlayMusic(Sound.MAINMUSIC, 0.25f, true);
    }
}
