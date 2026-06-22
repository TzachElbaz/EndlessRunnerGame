using UnityEngine;
using UnityEngine.UIElements;

public class rebound : MonoBehaviour
{
    Player player;
    public float depth = 1;
    public bool _isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            float realVelocity = player.velocity.x / depth;
            Vector2 position = transform.position;
            position.x += realVelocity * Time.deltaTime;
            transform.position = position;
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            Parallax ob =  gameObject.GetComponent<Parallax>();
            ob._startPosition=transform.position;
            ob.enabled = false;
            _isActive = true;
           
        }
    }
}
