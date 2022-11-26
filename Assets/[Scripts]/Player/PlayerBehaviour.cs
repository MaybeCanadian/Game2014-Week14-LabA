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


    [Header("Animations")]
    public Animator animator;
    public PlayerAnimationState playerAnimState;

    [Header("Controls")]
    public Joystick leftStick;
    [Range(0.0f, 1.0f)]
    public float verticalThreshold;

    private Rigidbody2D rb;

    private bool UseMobileInput = false;

    // Start is called before the first frame update
    void Start()
    {
        UseMobileInput = Application.isMobilePlatform;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        leftStick = (UseMobileInput) ? GameObject.Find("Left Stick").GetComponent<Joystick>() : null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;

        Vector2 input = (UseMobileInput ? GetMobileInput() : GetKeyboardInput());

        Move(input.x);
        Jump(input.y);
        AirCheck();
    }

    private Vector2 GetMobileInput()
    {
        return new Vector2(leftStick.Horizontal, leftStick.Vertical);
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

            ChangeAnimation(PlayerAnimationState.RUN);
        }

        if(isGrounded && x == 0)
        {
            ChangeAnimation(PlayerAnimationState.IDLE);
        }
    }

    private void Jump(float y)
    {        
        if(isGrounded && y > verticalThreshold)
        {
            rb.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
        }
    }

    private void AirCheck()
    {
        if(!isGrounded)
        {
            ChangeAnimation(PlayerAnimationState.JUMP);
        }
    }

    public void Flip(float x)
    {
        if (x != 0.0f)
        {
            transform.localScale = new Vector3((x > 0.0f) ? 1.0f : -1.0f, 1.0f, 1.0f); 
        }
    }

    private void ChangeAnimation(PlayerAnimationState state)
    {
        playerAnimState = state;

        animator.SetInteger("AnimState", (int)playerAnimState);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }
}
