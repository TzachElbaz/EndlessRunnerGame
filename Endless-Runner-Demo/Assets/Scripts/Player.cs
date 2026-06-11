using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action<int> OnPlayerHit;
    public static event Action OnPlayerDied;

    private Rigidbody2D _rigidbody;
    private Animator _animation;
    private BoxCollider2D _boxCollider;

    [Header("Jumping")]
    [SerializeField] public float firstJumpForce = 25f;
    [SerializeField] public float doubleJumpForce = 30f;
    private int jumpRemaining = 2;

    [Header("Running")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxXVelocity = 100f;

    [Header("Collision")]
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private Color _groundCheckColor;
    private Vector2 standingSize;
    private Vector2 rollingSize;
    private Vector2 standingOffset;
    private Vector2 rollingOffset;
    private Vector2 jumpingOffset = new Vector2(-0.02f, 0.57f);
    private Vector2 jumpingSize = new Vector2(0.61f, 0.74f);

    [Header("Rolling")]
    [SerializeField] private float rollDuration = .6f;
    [SerializeField] private float fastFallVelocity = 40f;
    private bool isRolling = false;
    public bool isGrounded;
    private bool isDoubleJumping = false;
    private bool rollOnLand = false;

    //[HideInInspector]
    public Vector2 velocity;
    [HideInInspector]
    public float distance = 0f;
    [HideInInspector]
    public float _collectableSpawnClock;
    [HideInInspector]
    public int health = 3;

    [Header("debug")]
    [SerializeField] private bool _hpOn;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animation = GetComponentInChildren<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        velocity.x = 15f;
    }

    private void Start()
    {
        SetColliders();
    }

    private void Update()
    {
        HandleInput();
        PlayerAnimation();
    }

    private void FixedUpdate()
    {

        float velocityRatio = velocity.x / maxXVelocity;
        float currentAcceleration = acceleration * (1 - velocityRatio);

        velocity.x += currentAcceleration * Time.fixedDeltaTime;
        velocity.x = Mathf.Min(velocity.x, maxXVelocity);

        distance += velocity.x * Time.fixedDeltaTime;
        _collectableSpawnClock += velocity.x * Time.fixedDeltaTime;
    }

    private void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            && CanPlayerJump())
        {
            if (isRolling)
            {
                StopCoroutine(RollRutine());
                EndRolling();
            }
            isGrounded = false;
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (isGrounded)
            {
                Roll();
            }
            else
            {
                // Air roll: force player down and set flag to roll on landing
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, -Mathf.Abs(fastFallVelocity));
                rollOnLand = true;
            }
        }
    }

    private void Jump()
    {
        jumpRemaining -= 1;
        if (jumpRemaining == 0)
            isDoubleJumping = true;

        float jumpingForce = jumpRemaining == 1 ? firstJumpForce : doubleJumpForce;
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);
        _rigidbody.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
        _boxCollider.offset = jumpingOffset;
        _boxCollider.size = jumpingSize;
    }


    //private bool GroundCheck()
    //{
    //    Vector2 origin = new Vector2(transform.position.x, transform.position.y - _groundCheckDistance);

    //    RaycastHit2D hit = Physics2D.BoxCast(origin, _groundCheckSize, 0f, Vector2.down, _groundCheckDistance, _groundLayers);
    //    return hit.collider != null;
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = _groundCheckColor;
    //    Vector2 origin = new Vector2(transform.position.x, transform.position.y - _groundCheckDistance);
    //    Gizmos.DrawWireCube(origin, _groundCheckSize);
    //}

    private bool GroundCheck()
    {
        Vector2 origin = new Vector2(_boxCollider.bounds.center.x, _boxCollider.bounds.min.y);

        RaycastHit2D hit = Physics2D.BoxCast(origin, _groundCheckSize, 0f, Vector2.down, _groundCheckDistance, _groundLayers);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        BoxCollider2D col = _boxCollider != null ? _boxCollider : GetComponent<BoxCollider2D>();
        if (col == null) return;

        Gizmos.color = _groundCheckColor;

        Vector2 origin = new Vector2(col.bounds.center.x, col.bounds.min.y);
        Gizmos.DrawWireCube(origin + (Vector2.down * _groundCheckDistance), _groundCheckSize);
    }
    private void PlayerAnimation()
    {
        _animation.SetBool("isRolling", isRolling);
        _animation.SetBool("isGrounded", isGrounded);
        _animation.SetBool("isDoubleJumping", isDoubleJumping);
    }

    private void Roll()
    {
        if (!isGrounded)
            return;

        _boxCollider.size = rollingSize;
        _boxCollider.offset = rollingOffset;
        StopAllCoroutines();
        StartCoroutine(RollRutine());
    }

    private IEnumerator RollRutine()
    {
        isRolling = true;
        yield return new WaitForSeconds(rollDuration);
        EndRolling();
    }

    private void EndRolling()
    {
        isRolling = false;
        if (isGrounded)
        {
            _boxCollider.size = standingSize;
            _boxCollider.offset = standingOffset;
        }
    }

    private void SetColliders()
    {
        standingSize = new Vector2(_boxCollider.size.x, _boxCollider.size.y);
        standingOffset = new Vector2(_boxCollider.offset.x, _boxCollider.offset.y);
        rollingSize = new Vector2(_boxCollider.size.x, _boxCollider.size.y / 2);
        rollingOffset = new Vector2(_boxCollider.offset.x, _boxCollider.offset.y / 2);
    }

    private bool CanPlayerJump()
    {
        return isGrounded || jumpRemaining > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && GroundCheck())
        {
            isGrounded = true;
            isDoubleJumping = false;
            jumpRemaining = 2;
            if (!isRolling)
            {
                _boxCollider.offset = standingOffset;
                _boxCollider.size = standingSize;
            }


            if (rollOnLand)
            {
                rollOnLand = false;
                Roll();
            }
            return;
        }

        if (collision.gameObject.CompareTag("obstacle"))
        {
            Debug.Log(collision.gameObject.GetComponentInParent<Obstacle>()._passPoint);
            //RunGameManeger.Instance.InvokeClearOnScreenObstacles();
            Destroy(collision.gameObject);
            if (health >= 1 && _hpOn)
            {
                health -= 1;
                OnPlayerHit?.Invoke(health);
                if (health == 0)
                    OnPlayerDied?.Invoke();
            }
        }
    }

}