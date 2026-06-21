using UnityEngine;

public class StelagtiteScript : MonoBehaviour
{
    public float _fallPoint;
    public float _fallSpeed;
    public float _deathPoint;
    [SerializeField] private GameObject _upST;
    [SerializeField] private GameObject _downST;
    [SerializeField] private bool _canFall;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_canFall && transform.position.x <= _fallPoint)
        {
            _downST.GetComponent<Rigidbody2D>().gravityScale = _fallSpeed;
        }
        if (transform.position.x <= _deathPoint)
        {
            Destroy(gameObject);
        }
    }
}
