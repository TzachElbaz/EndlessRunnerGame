using UnityEngine;

public class Platform : MonoBehaviour
{
    Player player;
    [SerializeField] private float depth = 1;
    [SerializeField] private float destroy = -30;
    [SerializeField] private float spawn = 75;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private TutorialManager TManager;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private Color _groundCheckColor;
    [SerializeField] private LayerMask _groundLayers;
    public bool isMooving = false;
    private bool needToSpawn = true;
    private BoxCollider2D _boxCollider;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!isMooving)
            return;
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        position.x -= realVelocity * Time.deltaTime;

        if (position.x < destroy && needToSpawn)
            position.x = spawn;

        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GroundCheck())
        {
            TManager.isJumpOnPlatform = true;
            needToSpawn = false;
            player.isGrounded = true;
        }
    }

    private bool GroundCheck()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _groundCheckDistance);

        RaycastHit2D hit = Physics2D.BoxCast(origin, _groundCheckSize, 0f, Vector2.down, _groundCheckDistance);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _groundCheckColor;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _groundCheckDistance);
        Gizmos.DrawWireCube(origin, _groundCheckSize);
    }

}
