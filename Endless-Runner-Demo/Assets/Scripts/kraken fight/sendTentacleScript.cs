using UnityEngine;

public class sendTentacleScript : MonoBehaviour
{
    [SerializeField] public float _tentacleSendTime;
    private float _tentacleSendClock = 0;
    [SerializeField] public float _tentacleSendspeed;
    [SerializeField] public float _warningTime1;
    [SerializeField] public float _attackHight;
    public bool _isActive =false;
    [SerializeField] public GameObject _debri;
    public bool _isDebri;
    public Vector2 _force;
    public Vector2 _debrySpawn= new Vector2(40, 1);
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
            //transform.position = new Vector2(30, -23);
        }
        _tentacleSendClock += Time.deltaTime;
        //Debug.Log(_tentacleSendClock);
        if (transform.position.y < _attackHight)
        {
           
            tentLocation.y += _tentacleSendspeed * Time.deltaTime;
            transform.position = tentLocation;
            if (_isDebri && transform.position.y>-18)
            {
                _isDebri = false;
                SendDebry();
            }
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
    private void SendDebry()
    {
        GameObject Ob;
        Ob = Instantiate(_debri);
        Ob.transform.position = _debrySpawn;
        Ob.GetComponent<Rigidbody2D>().AddForce( _force, ForceMode2D.Impulse);
    }
}
