using UnityEngine;

public class Parallax : MonoBehaviour
{
    Player player;
    public float depth = 1;
    public float destroy = -30;
    public float spawn = 75;
    public bool _yMove= false;
    public Vector2 _startPosition;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        if (!_yMove)
        {
            position.x -= realVelocity * Time.deltaTime;

            if (position.x < destroy)
                position.x = spawn;
        }
        else
        {
            position.y += realVelocity * Time.deltaTime;

            if (position.y > destroy)
                position.y = spawn;
        }
            transform.position = position;
    }
    private void OnDisable()
    {
        transform.position = _startPosition;
    }

}
