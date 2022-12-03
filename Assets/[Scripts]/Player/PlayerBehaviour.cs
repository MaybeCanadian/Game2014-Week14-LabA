using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Dust Trail Effect")]
    public ParticleSystem dustTrail;
    public Color dustTrailColor;

    [Header("Screen Shake Properties")]
    public CinemachineVirtualCamera playerCam;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float shakeIntensity;
    public float shakeDuration;
    public float shakeTimer;
    public bool isCameraShaking;

    [Header("Health System")]
    public HealthBarController health;
    public LifeCounterScript life;
    public DeathPlaneController deathPlane;
    // add life counter

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
        health = FindObjectOfType<PlayerHealth>().GetComponent<HealthBarController>();
        life = FindObjectOfType<LifeCounterScript>();
        deathPlane = FindObjectOfType<DeathPlaneController>();
        leftStick = (UseMobileInput) ? GameObject.Find("Left Stick").GetComponent<Joystick>() : null;

        dustTrail = GetComponentInChildren<ParticleSystem>();

        playerCam = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();

        perlin = playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        isCameraShaking = false;
        shakeTimer = shakeDuration;
        perlin.m_AmplitudeGain = 0.0f;

    }

    private void Update()
    {
        if(health.value <= 0)
        {
            Die();
        }
        
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

        //camera shake control

        if(isCameraShaking)
        {
            shakeTimer -= Time.fixedDeltaTime;
            if(shakeTimer <= 0)
            {
                perlin.m_AmplitudeGain = 0;
                shakeTimer = shakeDuration;
                isCameraShaking = false;
            }
        }
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

            if(isGrounded)
            {
                CreateDustTrail();
            }
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
            SoundManager.instance.PlaySoundFX(Sound.JUMP, Channel.PLAYER_SOUNDFX);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20);

            ShakeCamera();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Hazard"))
        {
            health.TakeDamage(30);

            ShakeCamera();
        }
    }

    public void Die()
    {
        life.LoseLife();

        if (life.value > 0)
        {
            health.ResetHealth();
            deathPlane.ReSpawn(gameObject);
            SoundManager.instance.PlaySoundFX(Sound.DEATH, Channel.PLAYER_DEATHFX);
        }
        else
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    public void TakeDamage(int value)
    {
        health.TakeDamage(value);

        SoundManager.instance.PlaySoundFX(Sound.HURT, Channel.PLAYER_HURTFX);
    }

    private void CreateDustTrail()
    {
        dustTrail.GetComponent<Renderer>().material.SetColor("_Color", dustTrailColor);
        dustTrail.Play();
    }

    private void ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeIntensity;
        isCameraShaking = true;
    }
}
