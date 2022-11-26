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
            var player = collision.GetComponent<PlayerBehaviour>();
            player.life.LoseLife();
            player.health.ResetHealth();

            if(player.life.value > 0)
            {
                SoundManager.instance.PlaySoundFX(Sound.HURT, Channel.PLAYER_HURTFX);
                ReSpawn(player.gameObject);
            }
            else
            {
                player.gameObject.SetActive(false);
                SoundManager.instance.PlaySoundFX(Sound.DEATH, Channel.PLAYER_DEATHFX);
            }
        }
    }

    public void ReSpawn(GameObject go)
    {
        go.transform.position = currentCheckpoint.position;
    }
}
