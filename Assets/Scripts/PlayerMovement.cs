using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Rigidbody2D rb;
    public TextMeshProUGUI scoreText;

    [Header("Settings")]
    public float jumpForce = 8f;
    public float slideDuration = 0.65f;
    public float dodgeDuration = 0.6f;

    [Header("Difficulty & Score")]
    public float scoreMultiplier = 10f;

    [HideInInspector] public float score = 0f;
    [HideInInspector] public float slideTimer = 0f;
    [HideInInspector] public float leftTimer = 0f;
    [HideInInspector] public float rightTimer = 0f;
    [HideInInspector] public bool isGrounded = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        HandleTimers();
        HandleScore();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            TriggerLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            TriggerRight();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            TriggerJump();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && slideTimer <= 0f)
        {
            TriggerSlide();
        }
    }
    public void TriggerJump()
    {
        rb.linearVelocity = new Vector2(0f, jumpForce);
        animator.SetTrigger("jumpTrigger");
    }

    public void TriggerSlide()
    {
        animator.SetTrigger("slideTrigger");
        slideTimer = slideDuration;
    }

    public void TriggerLeft()
    {
        animator.SetTrigger("leftTrigger");
        leftTimer = dodgeDuration;
    }

    public void TriggerRight()
    {
        animator.SetTrigger("rightTrigger");
        rightTimer = dodgeDuration;
    }

    private void HandleTimers()
    {
        if (slideTimer > 0f) slideTimer -= Time.deltaTime;
        if (leftTimer > 0f) leftTimer -= Time.deltaTime;
        if (rightTimer > 0f) rightTimer -= Time.deltaTime;
    }

    private void HandleScore()
    {
        score += Time.deltaTime * scoreMultiplier;
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}