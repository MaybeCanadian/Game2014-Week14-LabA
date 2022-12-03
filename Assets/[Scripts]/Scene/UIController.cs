using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public GameObject miniMap;

    private void Start()
    {
        miniMap = GameObject.Find("Minimap");

        miniMap?.SetActive(false);
    }
    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnYButtonPressed()
    {
        miniMap?.SetActive(!miniMap.activeInHierarchy);
    }
}
