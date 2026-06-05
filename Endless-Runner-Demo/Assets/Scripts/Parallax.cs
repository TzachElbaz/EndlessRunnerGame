using UnityEngine;

public class Parallax : MonoBehaviour
{
    Player player;
    [SerializeField] private float depth = 1;
    [SerializeField] private float destroy = -30;
    [SerializeField] private float spawn = 75;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        position.x -= realVelocity * Time.deltaTime;

        if(position.x < destroy)
            position.x = spawn;

        transform.position = position;
    }

}
