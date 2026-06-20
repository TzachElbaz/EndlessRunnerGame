using System.Collections;

using UnityEngine;
public class BossScript : MonoBehaviour
{
    private RunGameManeger _runGameManeger;
    private Player _player;
    public int _maxHp;
    public int _curentHp;
    private bool _tentacleSmashOn;
    private bool _tentacleSmashDownOn;
    private bool _tripleTentacleOn;
    private bool _tentacleSendOn;
    [SerializeField] GameObject _tentacle;

    [SerializeField] float _revelSpeed;
   

    [Header("Tentacle Smash")]
    [SerializeField] private GameObject _uperTentacleSmash;
    [SerializeField] private GameObject _lowerTentacleSmash;
    [SerializeField] private GameObject _uperBableSmash;
    [SerializeField] private GameObject _lowerBableSmash;
    [SerializeField] private Vector2 _uperTentacleSmashSpawn;
    [SerializeField] private Vector2 _lowerTentacleSmashSpawn;
    [SerializeField] private float _tentacleSmashTime;
    [SerializeField] private float _warningTime;

    private bool _uperTentacleSmashDown;
    private bool _lowerTentacleSmashUp;

    private float _uperTentacleSmashClock =0;
    private float _loweTentacleSmashClock = 0;
    [SerializeField] private float _tentacleSmashspeed;

    [Header("Tentacle send")]
    [SerializeField] private GameObject _tentacleSend1;
    [SerializeField] private float _tentacleSendspeed;
    [SerializeField] private float _warningTime1;
    [SerializeField] private float _tentacleSendTime;
    [SerializeField] public float[] _attackHight;
    private float _tentacleSendClock = 0;

    [Header("Tentacle trip")]
    [SerializeField] private GameObject _tripTentaclePerent;
    [SerializeField] private GameObject[] _tripTentacle;
    [SerializeField] private float _tripleTentacleTime;
    private float[] _tripleTentacleClock = new float [5];
    [SerializeField] private float _tripTentacleSpeed;
    [SerializeField] private float _tripleWarningSpeed;
    [SerializeField] private float _tripleWarningTime;
    private bool[] _teltacleStabForward = new bool[5];
    [SerializeField] private bool[] _TOn ;

    [Header("Rock Crash")] 
    [SerializeField] private GameObject _rockCrashOB;
    [SerializeField] private float _rokSmashForce;
    [SerializeField] private float _rokSmashGravity;
    [SerializeField] private Vector2 _rokSmashSpawn;

    [Header("Rock throw low")]
    [SerializeField] private GameObject _rockThrowLowOB;
    [SerializeField] private float _rokThrowLowForce;
    [SerializeField] private float _rokThrowLowGravity;
    [SerializeField] private Vector2 _rokThrowLowSpawn;
    [SerializeField] private Vector2 _rokThrowHighSpawn;

    private bool IsStartPositioning;


    private void Awake()
    {
        IsStartPositioning = true;
    }
    void Start()
    {
        _player = GameObject.FindAnyObjectByType<Player>();
        _runGameManeger = GameObject.FindAnyObjectByType<RunGameManeger>();
        
    }
    

    // Update is called once per frame
    void Update()
    {
        attackOveride();
        Timekiper();
        if (IsStartPositioning)
        {
            StartPositioning();
        }
    }
    private void Timekiper()
    {
        if (_tentacleSmashOn) TentacleSmash();
        if (_tentacleSmashDownOn) TentacleSmashDown();



        if (_TOn[4]) TripleTentacle(4);
        if (_TOn[0]) TripleTentacle(0);
        if (_TOn[1]) TripleTentacle(1);
        if (_TOn[2]) TripleTentacle(2);
        if (_TOn[3]) TripleTentacle(3);
    }
    
    private void TentacleSmash()
    {
        Vector2 tnetLocation = new Vector2(10f, _uperTentacleSmash.transform.position.y);
        if ( _uperTentacleSmashClock == 0)
        {
            _uperTentacleSmash.transform.position = new Vector2(10f,54f);
            
            _uperTentacleSmashDown = true;
            
            _uperBableSmash.transform.position = new Vector2(10f,28f);
        }
        else if(_uperTentacleSmashClock >= _tentacleSmashTime)
        {
            _uperTentacleSmash.transform.position = new Vector2(10f, 54f);
            
            _uperTentacleSmashClock = 0;
            _tentacleSmashOn = false;
        }
        _uperTentacleSmashClock += Time.deltaTime;
        if (_uperTentacleSmashClock >= _warningTime-0.02f) //buble finisg
        {
            _uperBableSmash.transform.position = new Vector2(10f, 100f);
        }

        if (_uperTentacleSmash.transform.position.y <= 28f) //revers movment
        {
            _uperTentacleSmashDown = false;
        }

        if (_uperTentacleSmashDown && _uperTentacleSmashClock>= _warningTime)
        {
            tnetLocation.y -= _tentacleSmashspeed * Time.deltaTime;
            _uperTentacleSmash.transform.position = new Vector2(10f, tnetLocation.y);
        }
        else if (!_uperTentacleSmashDown && _uperTentacleSmashClock >= _warningTime)
        {
            tnetLocation.y += _tentacleSmashspeed * Time.deltaTime;
            _uperTentacleSmash.transform.position = new Vector2(10f, tnetLocation.y);
        }

        if (_uperTentacleSmash.transform.position.y > 55f)
        {
            _tentacleSmashOn = false;
            _uperTentacleSmashClock = 0;
        }

    }
    private void TentacleSmashDown()
    {
        Vector2 tnetLocation = new Vector2(10f, _lowerTentacleSmash.transform.position.y);
        if (_loweTentacleSmashClock == 0)
        {
           
            _lowerTentacleSmash.transform.position = new Vector2(10f, -23f);
            
            _lowerTentacleSmashUp = true;
            _lowerBableSmash.transform.position = new Vector2(10f,-12f);
        }
        else if (_loweTentacleSmashClock >= _tentacleSmashTime)
        {
            
            _lowerTentacleSmash.transform.position = new Vector2(10f, -23f);
            _loweTentacleSmashClock = 0;
            _tentacleSmashDownOn = false;
        }
        _loweTentacleSmashClock += Time.deltaTime;
        if (_loweTentacleSmashClock >= _warningTime - 0.02f) //buble finisg
        {
            _lowerBableSmash.transform.position = new Vector2(10f, 100f);
        }

        if (_lowerTentacleSmash.transform.position.y >= -12f) //revers movment
        {
            _lowerTentacleSmashUp = false;
        }

        if (!_lowerTentacleSmashUp && _loweTentacleSmashClock >= _warningTime)
        {
            tnetLocation.y -= _tentacleSmashspeed * Time.deltaTime;
            _lowerTentacleSmash.transform.position = new Vector2(10f, tnetLocation.y);
        }
        else if (_lowerTentacleSmashUp && _loweTentacleSmashClock >= _warningTime)
        {
            tnetLocation.y += _tentacleSmashspeed * Time.deltaTime;
            _lowerTentacleSmash.transform.position = new Vector2(10f, tnetLocation.y);
        }

        if (_lowerTentacleSmash.transform.position.y < -24f)
        {
            _tentacleSmashDownOn = false;
            _loweTentacleSmashClock = 0;
        }
    }
   
    private void TentacleSend(int higt)
    {
        GameObject Ob;
        Ob = Instantiate(_tentacleSend1);
        Ob.transform.position = new Vector2(30, -23);
        sendTentacleScript tent = Ob.GetComponent<sendTentacleScript>();
        tent._tentacleSendTime = _tentacleSendTime;
        tent._tentacleSendspeed= _tentacleSendspeed;
        tent._warningTime1= _warningTime1;
        tent._isActive= true;
        tent._attackHight = _attackHight[higt];
    }
    private void TripleTentacle(int lv)
    {
        

        if ( _tripleTentacleClock[lv] == 0)
        {
            _tripTentaclePerent.transform.position = new Vector2(77, 6);
            _teltacleStabForward[lv] = true;
            _tripTentacle[lv].transform.position = new Vector2(77-6, _tripTentacle[lv].transform.position.y);
        }
        Vector2 tentLocation = _tripTentacle[lv].transform.position;


        _tripleTentacleClock[lv] += Time.deltaTime;
        if (_tripleTentacleClock[lv] < _tripleWarningTime && tentLocation.x > 67)
        {
            tentLocation.x -= _tripleWarningSpeed * Time.deltaTime;
            _tripTentacle[lv].transform.position = tentLocation;
        }
        if(_tripleTentacleClock[lv] > _tripleWarningTime  && _teltacleStabForward[lv])
        {
            tentLocation.x -= _tripTentacleSpeed * Time.deltaTime;
            _tripTentacle[lv].transform.position = tentLocation;
        }
        if (tentLocation.x < 27)
        {
            _teltacleStabForward[lv] = false;
        }
        if (_tripleTentacleClock[lv] > _tripleWarningTime && !_teltacleStabForward[lv])
        {
            tentLocation.x += _tripTentacleSpeed * Time.deltaTime;
            _tripTentacle[lv].transform.position = tentLocation;
        }
        if (tentLocation.x > 71 && !_teltacleStabForward[lv])
        {
            _TOn[lv] = false;
            _tripleTentacleClock[lv] = 0;
            _tripTentacle[lv].transform.position = new Vector2(77 - 6, _tripTentacle[lv].transform.position.y);
        }

    }

    private void RockKrash()
    {
        //_rockCrashOB.GetComponent<Rigidbody2D>().AddForceY(1000);
        _rockCrashOB.transform.position = _rokSmashSpawn;
        _rockCrashOB.GetComponent<Parallax>().enabled = true;
        _rockCrashOB.GetComponent<Rigidbody2D>().gravityScale = _rokSmashGravity;
        _rockCrashOB.GetComponent<Rigidbody2D>().AddForceY(_rokSmashForce, ForceMode2D.Impulse);
        Debug.Log("glap");
    }

    private void RockThrowLow(Vector2 span)
    {
        _rockThrowLowOB.transform.position = span;
        _rockThrowLowOB.GetComponent<Parallax>().enabled = true;
        _rockThrowLowOB.GetComponent<Rigidbody2D>().gravityScale = _rokThrowLowGravity;
        _rockThrowLowOB.GetComponent<Rigidbody2D>().AddForceY(_rokThrowLowForce, ForceMode2D.Impulse);
    }
    private void attackOveride()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            _tentacleSmashOn =true;
           
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _tentacleSmashDownOn = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            TentacleSend(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            _tripleTentacleOn = !_tripleTentacleOn;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            RockKrash();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            RockThrowLow(_rokThrowLowSpawn);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            RockThrowLow(_rokThrowHighSpawn);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _TOn[0] = !_TOn[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _TOn[1] = !_TOn[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _TOn[2] = !_TOn[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _TOn[3] = !_TOn[3];
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _TOn[4] = !_TOn[4];
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(AttackPatern1());
        }
    }
    IEnumerator AttackPatern1()
    {
        TentacleSend(1);
        yield return new WaitForSeconds(1.5f);
        TentacleSend(0);
        yield return new WaitForSeconds(4f);
        _tentacleSmashOn = true;
        yield return new WaitForSeconds(3f);
        _tentacleSmashOn = true;
        yield return new WaitForSeconds(2f);
        _TOn[2] = true;
        yield return new WaitForSeconds(2f);
        _TOn[3] = true;
        yield return new WaitForSeconds(2f);
        _TOn[3] = true;
        _TOn[4] = true;
        yield return new WaitForSeconds(2f);
        _TOn[1] = true;
        _TOn[4]=true;
        _TOn[3]=true;
        yield return new WaitForSeconds(2f);
        _TOn[0] = true;
        _TOn[3] = true;
        _TOn[4] = true;
        yield return new WaitForSeconds(2f);
        _TOn[0] = true;
        _TOn[1]=true;
        _TOn[2]=true;
        _TOn[3] = true;
        yield return new WaitForSeconds(2f);
        _tentacleSmashDownOn = true;
        yield return new WaitForSeconds(3f);
        _tentacleSmashDownOn = true;
        yield return new WaitForSeconds(0.7f);
        _tentacleSmashOn=true;

    }

    private void StartPositioning()
    {
        float move = transform.position.x- _revelSpeed* Time.deltaTime;
        transform.position = new Vector2(move,transform.position.y);
        if(transform.position.x< 53.4f) IsStartPositioning = false;
    }
}
