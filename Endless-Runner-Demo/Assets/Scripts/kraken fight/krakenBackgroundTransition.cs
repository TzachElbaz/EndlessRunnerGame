using UnityEngine;
using UnityEngine.UIElements;

public class krakenBackgroundTransition : MonoBehaviour
{
    Player player;
    [SerializeField] private float depth = 1;
    [SerializeField] private float stop = -30;
    [SerializeField] private float repose = 75;
    public bool _continu=false;
    private Vector2 _startPosition;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        Debug.Log(position.y);

        if (position.y + 2.69 < stop)
        {
            position.y += realVelocity * Time.deltaTime;
            transform.position = position;
        }
        else if (_continu)
        {
            position.y += realVelocity * Time.deltaTime;
            transform.position = position;
        }
        if (position.y + 2.69 > repose) 
        {
            position = _startPosition;
            gameObject.SetActive(false);
        }
        //position.y = spawn;
        Debug.Log(position.y + "k");

    }
}
