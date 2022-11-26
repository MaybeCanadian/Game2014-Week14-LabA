using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class PlayerDetection : MonoBehaviour
{
    [Header("Sensing Suite")]
    private Collider2D colliderName;
    public LayerMask collidionLayerMask;
    public bool playerDetected = false;
    public bool lineOfSight = false;
    private Transform playerTransform;
    public float playerDirection = 0;
    public Vector2 enemyDirection;
    public Vector2 playerDetectionVector;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(playerDetected)
        {
            var hit = Physics2D.Linecast(transform.position, playerTransform.position, collidionLayerMask);

            colliderName = hit.collider;


            playerDetectionVector = (playerTransform.position - transform.position).normalized;
            playerDirection = (playerDetectionVector.x > 0) ? 1.0f : -1.0f;
            enemyDirection = GetComponentInParent<EnemyController>().direction;

            lineOfSight = (hit.collider.tag == "Player") && (playerDirection == enemyDirection.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerDetected = true;
            playerTransform = collision.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (lineOfSight) ? Color.green : Color.red;

        if(playerDetected)
        {
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }

        Gizmos.color = (playerDetected) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 15.0f);
    }
}
