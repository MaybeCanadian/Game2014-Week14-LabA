using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackAction : MonoBehaviour, Action
{
    public bool hasLOS;
    [Range(1, 100)]
    public int fireDelay = 20;
    public Transform bulletSpawn;
    public Vector2 targetOffset;
    public GameObject bulletPrefab;
    public Transform bulletParent;

    private void Awake()
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    }
    public void Update()
    {
        hasLOS = transform.parent.GetComponentInChildren<PlayerDetection>().lineOfSight;
    }

    private void FixedUpdate()
    {
        if(hasLOS && Time.frameCount % fireDelay == 0) 
        {
            Execute();
        }
    }
    public void Execute()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity, bulletParent);

        bullet.GetComponent<BulletController>().Activate();

        //To Do -- bullet sound
    }
}
