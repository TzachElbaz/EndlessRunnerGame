using UnityEngine;

public class sendTentacleScript : MonoBehaviour
{
    [SerializeField] public float _tentacleSendTime;
    private float _tentacleSendClock = 0;
    [SerializeField] public float _tentacleSendspeed;
    [SerializeField] public float _warningTime1;
    [SerializeField] public float _attackHight;
    public bool _isActive =false;
    void Update()
    {
        Movment();
    }
    private void Movment()
    {
        if (!_isActive) return;

        Vector2 tentLocation = transform.position;
        if (_tentacleSendClock == 0)
        {
            transform.position = new Vector2(30, -23);
        }
        _tentacleSendClock += Time.deltaTime;
        //Debug.Log(_tentacleSendClock);
        if (transform.position.y < _attackHight)
        {
            tentLocation.y += _tentacleSendspeed * Time.deltaTime;
            transform.position = tentLocation;
        }
        if (_tentacleSendClock > _warningTime1)
        {
            tentLocation.x -= _tentacleSendspeed * Time.deltaTime;
            transform.position = tentLocation;
        }
        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }
}
