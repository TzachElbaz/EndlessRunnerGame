using UnityEngine;

public class Parallax : MonoBehaviour
{
    Player player;
    public float depth = 1;
    public float destroy = -30;
    public float spawn = 75;
    public bool _yMove= false;
    public bool _back= false;
    public Vector2 _startPosition;
    public bool _willDelet;
    public bool _willDisable= false;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        if (!_back)
        {
            if (!_yMove)
            {
                position.x -= realVelocity * Time.deltaTime;
                if (position.x < destroy && _willDelet) Destroy(gameObject);
                else if (position.x < destroy && _willDisable) gameObject.SetActive(false);
                if (position.x < destroy)
                    position.x = spawn;
            }
            else
            {
                position.y += realVelocity * Time.deltaTime;
                if (position.y > destroy && _willDelet) Destroy(gameObject);
                else if (position.y > destroy && _willDisable) gameObject.SetActive(false);

                if (position.y > destroy) position.y = spawn;
            }
        }
        else
        {
            if (!_yMove)
            {
                position.x += realVelocity * Time.deltaTime;
                if (position.x > destroy && _willDelet) Destroy(gameObject);
                else if (position.x > destroy && _willDisable) gameObject.SetActive(false);
                if (position.x > destroy)
                    position.x = spawn;
            }
            else
            {
                position.y -= realVelocity * Time.deltaTime;
                if (position.y < destroy && _willDelet) Destroy(gameObject);
                else if(position.y < destroy && _willDisable) gameObject.SetActive(false);

                if (position.y < destroy) position.y = spawn;
            }
        }
            transform.position = position;
    }
    private void OnDisable()
    {
        transform.position = _startPosition;
    }

}
