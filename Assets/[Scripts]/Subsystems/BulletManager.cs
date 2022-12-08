using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    public List<BulletPool> bulletPools;

    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        Initialize();
    }

    private void Initialize()
    {
        BuildBulletPools();
    }

    private void BuildBulletPools()
    {
        //if(bulletPools == null)
        //{
        //    bulletPools = new List<BulletPool>();
        //}

        //for (int i = 0; i < Enum.GetValues(typeof(BulletTypes)).Length; i++)
        //{
        //    if(bulletPools.Count != i - 1)
        //    {
        //        bulletPools.Add(new BulletPool());
        //    }
        //}

        foreach (BulletPool pool in bulletPools)
        {
            pool.queue = new Queue<GameObject>();

            for (int i = 0; i < pool.startingAmount; i++)
            {
                pool.queue.Enqueue(BulletFactory.instance.CreateBullet(BulletTypes.ACORN));
                pool.remainingAmount = pool.queue.Count;
            }

            pool.activeAmount = 0;
            pool.remainingAmount = pool.queue.Count;
        }
    }

    public GameObject GetBullet(BulletTypes type)
    {
        if((int)type >= bulletPools.Count)
        {
            Debug.LogError("There is no pool for that type.");
            return null;
        }

        if (bulletPools[(int)type].queue.Count <= 0)
        {
            bulletPools[(int)type].queue.Enqueue(BulletFactory.instance.CreateBullet(BulletTypes.ACORN));
            bulletPools[(int)type].remainingAmount = bulletPools[(int)type].queue.Count;
        }

        var bullet = bulletPools[(int)type].queue.Dequeue();
        bullet.SetActive(true);
        bulletPools[(int)type].activeAmount++;
        bulletPools[(int)type].remainingAmount = bulletPools[(int)type].queue.Count;
        return bullet;
    }

    public void ReturnBullet(BulletTypes type, GameObject bullet)
    {
        if ((int)type >= bulletPools.Count)
        {
            Debug.LogError("There is no pool for that type");
            return;
        }

        bullet.SetActive(false);
        bullet.GetComponent<BulletController>().rb.velocity = new Vector3(0, 0, 0);
        bullet.GetComponent<BulletController>().rb.angularVelocity = 0.0f;
        bulletPools[(int)type].queue.Enqueue(bullet);
        bulletPools[(int)type].activeAmount--;
        bulletPools[(int)type].remainingAmount = bulletPools[(int)type].queue.Count;
    }
}

[System.Serializable]
public class BulletPool
{
    public Queue<GameObject> queue;
    public int startingAmount = 50;
    public int activeAmount = 0;
    public int remainingAmount = 0;
}
