using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject OnScreenControls;

    public GameObject miniMap;

    public bool enableMusic = true;

    private void Awake()
    {
        OnScreenControls = GameObject.Find("OnScreenControls");

        OnScreenControls.SetActive(Application.isMobilePlatform);

        if(enableMusic)
            SoundManager.instance.PlayMusic(Sound.MAINMUSIC, 0.25f, true);

        miniMap = GameObject.Find("Minimap");
        miniMap?.SetActive(false);
    }

    private void Update()
    {
        if(miniMap && Input.GetKeyDown(KeyCode.M)) {
            miniMap?.SetActive(!miniMap.activeInHierarchy);
        }
    }
}
