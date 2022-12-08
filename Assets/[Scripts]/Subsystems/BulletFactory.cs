using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory instance;

    private List<GameObject> bulletPrefabs;
    private Transform bulletParent;
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

        InitPrefabs();
        bulletParent = GameObject.Find("[Bullets]").transform;
    }

    private void InitPrefabs()
    {
        bulletPrefabs = new List<GameObject>();
        bulletPrefabs.Add(Resources.Load<GameObject>("Prefabs/Bullet"));
    }

    public GameObject CreateBullet(BulletTypes type)
    {
        GameObject tempBullet = Instantiate(bulletPrefabs[(int)type], bulletParent);
        tempBullet.SetActive(false);
        return tempBullet;
    }
}

public enum BulletTypes
{
    ACORN
}
