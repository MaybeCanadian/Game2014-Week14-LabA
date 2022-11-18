using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public PlatformDirection direction;

    [Header("Movement Properties")]
    [Range(1.0f, 20.0f)]
    public float horizontalRange = 8.0f;
    [Range(1.0f, 20.0f)]
    public float horizontalSpeed = 3.0f;
    [Range(1.0f, 20.0f)]
    public float verticalRange = 8.0f;
    [Range(1.0f, 20.0f)]
    public float verticalSpeed = 3.0f;
    [Range(0.001f, 0.1f)]
    public float customSpeedFactor = 0.3f;

    [Header("PathPoints")]
    public List<Transform> pathPoints;
    private Vector2 startPosition;
    private Vector2 destinationPoint;
    private float timer;
    private int currentPathIndex;

    private List<Vector2> pathList;
    void Start()
    {
        timer = 0.0f;
        currentPathIndex = 0;
        startPosition = transform.position;
        pathList = new List<Vector2>();
        foreach(Transform pathPoint in pathPoints)
        {
            Vector2 point = pathPoint.position;

            pathList.Add(point);
        }
        pathList.Add(transform.position);

        destinationPoint = pathList[currentPathIndex];
    }

    private void FixedUpdate()
    {
        if(direction == PlatformDirection.CUSTOM)
        {
            if (timer <= 1.0f)
            {
                timer += customSpeedFactor;
            }
            else if (timer >= 1.0f)
            {
                timer = 0.0f;

                currentPathIndex++;

                if (currentPathIndex >= pathList.Count)
                {
                    currentPathIndex = 0;
                }

                startPosition = transform.position;
                destinationPoint = pathList[currentPathIndex];
            }
        }
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
        transform.position = Vector2.Lerp(startPosition, destinationPoint, timer);
    }
}
