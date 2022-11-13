using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneController : MonoBehaviour
{
    public Transform currentCheckpoint;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ReSpawn(collision.gameObject);
        }
    }

    public void ReSpawn(GameObject go)
    {
        go.transform.position = currentCheckpoint.position;
    }
}
