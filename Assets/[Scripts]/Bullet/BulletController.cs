using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Properties")]
    public Vector2 direction;
    public Rigidbody2D rb;
    [Range(1.0f, 100.0f)]
    public float launchForce;
    public Vector2 rotationForce;
    public float lifeTime = 2.0f;
    public Vector3 Offset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = (GameObject.FindWithTag("Player").transform.position + Offset) - transform.position;
        direction = direction.normalized;
    }

    private void Start()
    {
        Activate();
    }
    public void Activate()
    {
        Rotate();
        Move();

        Invoke("Remove", lifeTime);
    }
    private void Rotate()
    {
        rb.AddTorque(Random.Range(rotationForce.x, rotationForce.y) * -1.0f * direction.normalized.x, ForceMode2D.Impulse);
    }
    private void Move()
    {
        //float force = 0.0f;
        
        rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
    }
    private void Remove()
    {
        if(gameObject.activeInHierarchy)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Remove();
        }
    }
}
