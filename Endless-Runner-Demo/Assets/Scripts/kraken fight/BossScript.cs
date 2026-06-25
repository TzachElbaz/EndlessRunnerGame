using NUnit.Framework;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
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
    private bool _IsDefet;
    [SerializeField] private Sprite _blanckHeart;
    [SerializeField] private Sprite _FillHeart;
    [SerializeField] private GameObject[] _hearts;
    [SerializeField] private GameObject _heartParent;
    [SerializeField] private GameObject _tentacle;
    [SerializeField] private Animator _animator;
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
    [SerializeField] private float _tentacleSendSPAWN;
    //private float _tentacleSendClock = 0;

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

    [Header("stalagtite fall")]
    [SerializeField] private GameObject[] _stelagtiteOB;

    private bool IsStartPositioning;

    [Header("rundom bulshit")]
    [SerializeField] private GameObject[] _bulshitOB;
    [SerializeField] private GameObject[] _bulshitGlowOB;
    private GameObject _curentBS;
    private GameObject _curentGlowBS;
    [SerializeField] private float _bulshitTime;
    [SerializeField] private GameObject _bulshitTentacle;
    [SerializeField] private Animator _BSanimation;
    [SerializeField] private float _bulshitTentacleSpawn;
    [SerializeField] private float _bulshitTentacleHigt;
    [SerializeField] private float _bulshitTentacleSpeed;
    [SerializeField] private float _bulshitTentacleRate;
    private float _bulshitClock;
    private bool _goingUp;
    public bool _isGlow;
    private bool _RBon=false;

    [Header("Platform send")]
    [SerializeField] private GameObject _PlatformSend1;
    [SerializeField] private float _PlatformSendspeed;
    [SerializeField] private float _PlatformWarningTime1;
    [SerializeField] private float _PlatformSendTime;
    [SerializeField] public float _PlatformAttackHight;
    [SerializeField] private float _PlatformSendSpawn;

    public int _faze;

    [Header("defet")]
    [SerializeField] private float _defetTime;
    private float _defetClock;


    private void Awake()
    {
        _animator.SetBool("is5", false);
        _animator.SetBool("is3", false);
        IsStartPositioning = true;
    }
    void Start()
    {
        _curentHp = _maxHp;
        for (int i = 0; i < 9; i++)
        {
            _hearts[i].GetComponent<Image>().sprite = _FillHeart;
        }
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
        if (_IsDefet)
        {
            Defet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("counter"))
        {
            Destroy(collision.gameObject);
            if (_curentHp > 0)
            {
                _animator.Play("kraken blink");
                _curentHp--;
                _hearts[_curentHp].GetComponent<Image>().sprite = _blanckHeart;
                if (_curentHp == 6) //seconde fase
                {
                    _animator.SetBool("is5", false);
                    _animator.SetBool("is3", true);
                }
                else if (_curentHp == 3) //third fase
                {
                    _animator.SetBool("is5", false);
                    _animator.SetBool("is3", false);
                }
                else if(_curentHp == 0)
                {
                    _IsDefet= false;
                   
                }

            }
            
        }
    }

    private void Timekiper()
    {
        if (_tentacleSmashOn) TentacleSmash();
        if (_tentacleSmashDownOn) TentacleSmashDown();

        if (_RBon) RunBulshit();


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
   
    private void TentacleSend(int higt, bool counter)
    {
        GameObject Ob;
        Ob = Instantiate(_tentacleSend1);
        Ob.transform.position = new Vector2(_tentacleSendSPAWN, -23);
        sendTentacleScript tent = Ob.GetComponent<sendTentacleScript>();
        tent._tentacleSendTime = _tentacleSendTime;
        tent._tentacleSendspeed= _tentacleSendspeed;
        tent._warningTime1= _warningTime1;
        tent._isActive= true;
        tent._attackHight = _attackHight[higt];
        tent._isDebri = counter;
    }
    private void TripleTentacle(int lv)
    {
        

        if ( _tripleTentacleClock[lv] == 0)
        {
            _tripTentaclePerent.transform.position = new Vector2(77, 5.7f);
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
    private void stalagtiteFall(int lv)
    {
        GameObject Ob;
        Ob = Instantiate(_stelagtiteOB[lv]);
        Ob.transform.position = new Vector2(68.5f, 23.16f);
    }

    private void RockKrash()
    {
        GameObject Ob;
        Ob = Instantiate(_rockCrashOB);
        Ob.transform.position = _rokSmashSpawn;
        Ob.GetComponent<Parallax>().enabled = true;
        Ob.GetComponent<Rigidbody2D>().gravityScale = _rokSmashGravity;
        Ob.GetComponent<Rigidbody2D>().AddForceY(_rokSmashForce, ForceMode2D.Impulse);
        Debug.Log("glap");
    }

    private void RockThrowLow(Vector2 span)
    {
        GameObject Ob;
        Ob = Instantiate(_rockThrowLowOB);
        Ob.transform.position = span;
        Ob.GetComponent<Parallax>().enabled = true;
        Ob.GetComponent<Rigidbody2D>().gravityScale = _rokThrowLowGravity;
        Ob.GetComponent<Rigidbody2D>().AddForceY(_rokThrowLowForce, ForceMode2D.Impulse);
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
            TentacleSend(0, true);
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
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
           stalagtiteFall(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            stalagtiteFall(1);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            stalagtiteFall(2);
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            TrhowBulshit(4, 2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlatformSend(_PlatformAttackHight, _PlatformSendSpawn);
        }
    }
   

    private void PlatformSend(float higt, float spawn)
    {
        GameObject Ob;
        Ob = Instantiate(_PlatformSend1);
        Ob.transform.position = new Vector2(spawn, -4);
        sendTentacleScript tent = Ob.GetComponent<sendTentacleScript>();
        tent._tentacleSendTime = _PlatformSendTime;
        tent._tentacleSendspeed = _PlatformSendspeed;
        tent._warningTime1 = _PlatformWarningTime1;
        tent._isActive = true;
        tent._attackHight = higt;
    }
    IEnumerator AttackPatern1()
    {
        TentacleSend(1, false); //f
        yield return new WaitForSeconds(1.5f);
        TentacleSend(0, true);
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
        if (transform.position.x < 48.5f)
        {
            IsStartPositioning = false;
            _animator.SetBool("is5",true);
            _heartParent.SetActive(true);
        }
    }
    public void ChooseBulshit()
    {
        int rn = Random.Range(0, _bulshitOB.Length);
        _curentBS = _bulshitOB[rn];
        _curentGlowBS =_bulshitGlowOB[rn];
        _curentBS.SetActive(true);
    }
    public void SpawnBulshit()
    {
        if (_isGlow)
        {
            GameObject ob;
            ob = Instantiate(_curentGlowBS);
            ob.GetComponent<Parallax>().enabled = true;
            ob.transform.position = _curentBS.transform.position;
            _isGlow = false;
        }
        else
        {
            GameObject ob;
            ob = Instantiate(_curentBS);
            ob.GetComponent<Parallax>().enabled = true;
            ob.transform.position = _curentBS.transform.position;
        }
        _BSlong--;
    }
    private int _BSlong;
    private int _BScounter;
    public void TrhowBulshit(int Long, int Counter)
    {

        _bulshitTentacle.SetActive(true);
        _RBon = true;
        _BSlong = Long;
        _BScounter = Counter;

    }
    public void RunBulshit()
    {
        if (_BSlong > 0)
        {
            _bulshitTentacle.GetComponent<Animator>().SetBool("isThrowing", true);
            if (_BSlong == _BScounter)
            {
                _isGlow = true;
            }

        }
        else 
        {
            _RBon= false;
            _bulshitTentacle.GetComponent<Animator>().SetBool("isThrowing", false);
        }
    }
    int defetHandeler = 0;
    private void Defet()
    {
        if (_defetClock ==0)
        {
            RunGameManeger.Instance.InvokeCangeErea();
        }
        
        if (transform.position.y < -17f)
        {
            IsStartPositioning = false;
            _animator.SetBool("is5", true);
            _heartParent.SetActive(true);
        }
        
        switch (defetHandeler)
        {
            case 0: // move down

                float move = transform.position.y - _revelSpeed * Time.deltaTime;
                transform.position = new Vector2(transform.position.x, move);
                if(transform.position.y < -17f) defetHandeler ++;
                break;

            case 1:
                RunGameManeger.Instance.InvokeCangeErea();
                break;

        }


    }


}
