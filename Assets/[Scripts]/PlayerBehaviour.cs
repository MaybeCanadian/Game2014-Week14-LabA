using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Properties")]
    public float horizontalForce;
    public float horizontalSpeed;
    public float verticalForce;
    public float airFactor;
    public Transform groundPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGrounded;

    private Rigidbody2D rb;

    public bool UseMobileInput = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;

        Vector2 input = (UseMobileInput ? GetMobileInput() : GetKeyboardInput());

        Move(input.x);
        Jump(input.y);
    }

    private Vector2 GetMobileInput()
    {
        Vector2 input = new Vector2(0.0f, 0.0f);

        if(Input.touches.Length > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 4.0f)
                {
                    input.x = -1.0f;
                }
                else if (touch.position.x > 3.0f * Screen.width / 4.0f)
                {
                    input.x = 1.0f;
                }
                else
                {
                    input.y = 1.0f;
                }
            }
        }

        return input;
    }

    private Vector2 GetKeyboardInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"));

        return input;
    }

    private void Move(float x)
    {
        if (x != 0.0f)
        {
            Flip(x);
            rb.AddForce(Vector2.right * x * horizontalForce * ((isGrounded) ? 1.0f : airFactor));

            var clampedXVeclocity = Mathf.Clamp(rb.velocity.x, -horizontalSpeed, horizontalSpeed);

            rb.velocity = new Vector2(clampedXVeclocity, rb.velocity.y);
        }
    }

    private void Jump(float y)
    {        
        if(isGrounded && y > 0.0f)
        {
            rb.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
        }
    }

    public void Flip(float x)
    {
        if (x != 0.0f)
        {
            transform.localScale = new Vector3((x > 0.0f) ? 1.0f : -1.0f, 1.0f, 1.0f); 
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }
}
