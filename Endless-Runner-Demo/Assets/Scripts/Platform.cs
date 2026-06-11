using UnityEngine;

public class Platform : MonoBehaviour
{
    Player player;
    [SerializeField] private float depth = 1;
    [SerializeField] private float destroy = -30;
    [SerializeField] private float spawn = 75;
    [SerializeField] private TutorialManager TManager;
    public bool isMooving = false;
    private bool needToSpawn = true;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
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
        if (collision.gameObject.CompareTag("Player"))
        {
            TManager.isJumpOnPlatform = true;
            needToSpawn = false;
        }
    }

}
