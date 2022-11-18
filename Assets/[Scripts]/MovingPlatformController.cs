using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public PlatformDirection direction;
    private Vector2 startPosition;

    [Header("Movement Properties")]
    [Range(1.0f, 20.0f)]
    public float horizontalRange = 8.0f;
    [Range(1.0f, 20.0f)]
    public float horizontalSpeed = 3.0f;
    [Range(1.0f, 20.0f)]
    public float verticalRange = 8.0f;
    [Range(1.0f, 20.0f)]
    public float verticalSpeed = 3.0f;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        switch(direction)
        {
            case PlatformDirection.HORIZONTAL:
                MoveHorizontal();
                break;
            case PlatformDirection.VERTICAL:
                MoveVertical();
                break;
            case PlatformDirection.DIAGONAL_UP:
                MoveDiagonal_UP();
                break;
            case PlatformDirection.DIAGONAL_DOWN:
                MoveDiagonal_Down();
                break;
            case PlatformDirection.CUSTOM:
                MoveCustom();
                break;
        }
    }

    public void MoveHorizontal()
    {
        transform.position = new Vector2(Mathf.PingPong(horizontalSpeed * Time.time, horizontalRange) + startPosition.x, 
            startPosition.y);
    }

    private void MoveVertical()
    {
        transform.position = new Vector2(startPosition.x,
            Mathf.PingPong(verticalSpeed * Time.time, verticalRange) + startPosition.y);
    }

    private void MoveDiagonal_UP()
    {
        transform.position = new Vector2(Mathf.PingPong(horizontalSpeed * Time.time, horizontalRange) + startPosition.x,
           Mathf.PingPong(verticalSpeed * Time.time, verticalRange) + startPosition.y);
    }

    private void MoveDiagonal_Down()
    {
        transform.position = new Vector2(Mathf.PingPong(horizontalSpeed * Time.time, horizontalRange) + startPosition.x,
           -Mathf.PingPong(verticalSpeed * Time.time, verticalRange) + startPosition.y);
    }

    private void MoveCustom()
    {

    }
}
