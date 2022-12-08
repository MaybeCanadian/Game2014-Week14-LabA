using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackAction : MonoBehaviour, Action
{
    [Header("Enemy Ranged Attack Properties")]
    private bool hasLOS;
    [Range(1, 100)]
    public Vector2 fireDelay = new Vector2(0.2f, 2.0f);
    public Transform bulletSpawn;
    private PlayerDetection playerDetec;

    float timer;
    float currentDelay;

    private void Awake()
    {
        
        timer = 0;
        currentDelay = Random.Range(fireDelay.x, fireDelay.y);
        playerDetec = transform.parent.GetComponentInChildren<PlayerDetection>();
        
    }
    public void Update()
    {
        hasLOS = playerDetec.lineOfSight;
    }

    private void FixedUpdate()
    {
        if(hasLOS && timer > currentDelay) 
        {
            currentDelay = Random.Range(fireDelay.x, fireDelay.y);
            timer = 0.0f;
            Execute();
        }

        timer += Time.fixedDeltaTime;
    }
    public void Execute()
    {
        var bullet = BulletManager.instance.GetBullet(BulletTypes.ACORN);
        bullet.transform.position = bulletSpawn.position;

        bullet.GetComponent<BulletController>().Activate();

        SoundManager.instance.PlaySoundFX(Sound.FIRE, 1.0f);

        //To Do -- bullet sound
    }
}
