using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject OnScreenControls;

    private void Awake()
    {
        OnScreenControls = GameObject.Find("OnScreenControls");

        OnScreenControls.SetActive(Application.isMobilePlatform);
    }
}
