using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Obstacle;
using Random = UnityEngine.Random;

public class RunGameManeger : MonoBehaviour
{
    public static bool isGamePaused = false;
    public static event Action OnEscapePressed;

    public static RunGameManeger Instance;

    public static event Action ClearAllObstacles;
    public static event Action ClearOffScreenObstacles;
    public static event Action ClearOnScreenObstacles;

    [SerializeField] private Player _Player;
    [SerializeField] private GameObject _PlayerObject;
    [SerializeField] private Vector2 _playerSpawn;

    [Header("Transition")]
    [SerializeField] private GameObject _forestBackground;
    [SerializeField] private GameObject[] _forestTransitionList;
    [SerializeField] private GameObject _desertBackground;
    [SerializeField] private GameObject[] _desertTransitionList;
    [SerializeField] private GameObject _krakenBackground;
    [SerializeField] private float _transitionTime;
    [SerializeField] private float _transitionSwitch;
    [SerializeField] private float _krakenRetransitionSpeed;
    private float _transitionClock;
    private bool _transitioning;
    [Header("Obstacles")]
    [SerializeField] private GameObject[] _forestObstecl;
    [SerializeField] private GameObject[] _forestObsteclCurse;
    [SerializeField] private GameObject[] _forestPlatforms;
    [SerializeField] private GameObject[] _desertObstecl;
    [SerializeField] private GameObject[] _desertObsteclCurse;
    [SerializeField] private GameObject[] _desertPlatforms;
    [SerializeField] private float _Xspon;
    [SerializeField] private float _Yspon;
    [SerializeField] private int _platformChance;
    [SerializeField] private int _obstacleCurseCount;


    public SCREEN_ENUM _curentScreen = SCREEN_ENUM.FOREST;
    public SCREEN_ENUM _nextScreen;

    [Header("alt generation")]
    [SerializeField] private int _pregenLength;
    [SerializeField] private float _minLength;
    [SerializeField] private float _addLength;
    private GameObject _LastObject;
    private int _obstacleCounter;
    private Vector2 _spawnPoint;


    
    
    GameObject[] _curentObstecl;
    GameObject[] _curentObsteclCours;
    GameObject[] _curentPlatforms;
    private GameObject[] pregen;
    private float[] genLength;
    [SerializeField, HideInInspector] public int[] pursegen;
    private int listCount;
    private bool _pregenEmpty;
    [SerializeField] private float _jumpChaineLength;
    [SerializeField] private float _dropChaineLength;
    
     private int _obstecalChainChance;
    [SerializeField] private int[] _obstecalChainChanceScale;
    [SerializeField] private int _obstecalChainChanceMax;
    [SerializeField] private int[] _obstecalChainMark;
    [SerializeField] private int _obsteclBrakeChance;

    public bool _obstaclePause;
    public bool _generateAlt;

    private bool isGameOver = false;
    private int pursePlace;
    private float _chineCounter;

    [Header("obstacle distant change")]
    [SerializeField] private float _velocityLengthAdd = 0f;
    [SerializeField] private float _velocityLengthAddThreshhold = 0f;

    [Header("coin generation")]
    [SerializeField] private int _coinChens;
    [SerializeField] private GameObject[] _coinList;
    [SerializeField] private Vector2Int _coinGenerationRange;


    [Header("kraken")]
    [SerializeField] private GameObject _kraken;
    [SerializeField] private Vector2 _krskenSpawn;
    [SerializeField] private GameObject _tentacleUp;
    [SerializeField] private Vector2 _tntacleUpSpawn;
    [SerializeField] private GameObject _tentacleDown;
    [SerializeField] private Vector2 _tentacleDownSpawn;
    [SerializeField] private GameObject _bubleUp;
    [SerializeField] private Vector2 _bubleUpSpawn;
    [SerializeField] private GameObject _bubleDown;
    [SerializeField] private Vector2 _bubleDownSpawn;
    [SerializeField] private GameObject _tripleTentaacle;
    [SerializeField] private Vector2 _tripleTentaacleSpawn;

    [SerializeField] private Vector2 _playerBossSpawn;

    [SerializeField] private GameObject _krakenTransition;
    [SerializeField] private Vector2 _krakenTransitionSpawn;
    [SerializeField] private GameObject _krakenBubleTransition;
    [SerializeField] private GameObject _krakenBubleTransition2;
    [SerializeField] private GameObject _krakenTentacleTranzition;
    [SerializeField] private Vector2 _krakenTentacleTranzitionSpawn;
    [SerializeField] private GameObject[] _krakenWaves;
    [SerializeField] private Vector2[] _waveSpawn;

    


    private bool test;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void OnEnable()
    {
        // Subscribe
        Player.OnPlayerDied += ShowGameOver;
    }
    private void OnDisable()
    {
        // Unsubscribe
        Player.OnPlayerDied -= ShowGameOver;
    }
    private void ShowGameOver()
    {
        Time.timeScale = 0f;
        isGameOver = true;
    }
    public enum SCREEN_ENUM
    {
        FOREST,
        DESERT,
        KRAKEN
    }

    void Start()
    {
        SetObstacleToErea();
        _spawnPoint.y = _Yspon;
        _spawnPoint.x = _Xspon;
        pregen = new GameObject[_pregenLength];
        genLength = new float[_pregenLength];
        pursegen = new int[_pregenLength];
        listCount = 0;
        _pregenEmpty = true;
        pregen[pregen.Length - 1] = _curentObstecl[0];
        _obstecalChainChance = _obstecalChainChanceScale[0];
        ;
        //CangeErea();
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            OnEscapePressed?.Invoke();
        }
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            return;
        }
        else
        {
            Time.timeScale = 1f;
        }
        ControllOveride();
        AddVelocityObsteclDistant();
        TimeKiper();
    }

    private void FixedUpdate()
    {
        if (!_generateAlt && SpawnCheck())
        {
            GenerateOb();
        }
        else if (_generateAlt && SpawnCheckAlt())
        {
            SpawnOB();
        }
        //_chineCounter += _Player.velocity.x * Time.fixedDeltaTime;
        //if (_chineCounter >= _obstecalChainMark && _obstecalChainChance< _obstecalChainChanceMax) 
        //{
        //    _obstecalChainChance++;
        //    _chineCounter = 0;
        //}

    }

    private void GenerateOb()
    {
        int rund;
        GameObject Ob;
        if (_obstacleCounter == _obstacleCurseCount)
        {
            rund = Random.Range(0, _curentObsteclCours.Length);
            Ob = Instantiate(_curentObsteclCours[rund]);

            _obstacleCounter = 0;
        }
        else
        {

            rund = Random.Range(0, _curentObstecl.Length);
            Ob = Instantiate(_curentObstecl[rund]);
            _obstacleCounter++;
        }
        Ob.transform.position = new Vector2(_spawnPoint.x, _spawnPoint.y);
        _LastObject = Ob;


    }
    private int _OBcount;
    private int _OBgenCount;
    private void GenerateObAlt()
    {
        //int prevLast =1;
        int rund;
        int repetCount = 0;
        float length = _minLength;
        int coinRange= Random.Range(_coinGenerationRange.x,_coinGenerationRange.y+1);
        int coinRangeCount=0;
        Obstacle now;
        Obstacle prev = pregen[pregen.Length - 1].GetComponent<Obstacle>();
        GameObject Ob;
        _OBcount = _OBgenCount;
        for (int i = 0; i < pregen.Length; i++)
        {
            if (_obstecalChainChanceMax > _obstecalChainChance)
            {
                for (int j = 1; j< _obstecalChainMark.Length; j++) 
                {
                    if (_OBcount >= _obstecalChainMark[j]) _obstecalChainChance = _obstecalChainChanceScale[j];
                }              
            }
            
            rund = Random.Range(0, _curentObstecl.Length);
            if (i > 0 && _curentObstecl[rund] == pregen[i - 1])
            {
                if (repetCount < 1) repetCount++;
                else
                {
                    repetCount = 0;
                    while (_curentObstecl[rund] == pregen[i - 1])
                    {
                        rund = Random.Range(0, _curentObstecl.Length);
                    }
                }

            }
            
            
            int randomObstacleEvent = Random.Range(0, 10);
            Ob = _curentObstecl[rund];
            pregen[i] = Ob;
            now = Ob.GetComponent<Obstacle>();
            if (i != 0)
            {
                prev = pregen[i - 1].GetComponent<Obstacle>();
            }
            pursePlace = 2;
            switch (prev._type)
            {
                case OB_TYPE.OBSTECLE:

                    
                    if (coinRangeCount == coinRange && _coinList[0] != null)
                    {
                        length = _minLength;
                        coinRangeCount = 0;
                        Ob = _coinList[Random.Range(0, _coinList.Length)];
                        pregen[i] = Ob;
                        now = Ob.GetComponent<Obstacle>();
                        

                        
                    }
                    else if (randomObstacleEvent <= _obstecalChainChance)
                    {
                        length = TwoOBDistantCheck(prev, now);
                    }
                    else if (randomObstacleEvent <= _obstecalChainChance + _obsteclBrakeChance)
                    {
                        length = _minLength * Random.Range(2, 5);
                    }
                    else if (randomObstacleEvent <= _obstecalChainChance + _obsteclBrakeChance+ _platformChance)
                    {
                        length = _minLength;
                        coinRangeCount = 0;
                        Ob = _curentPlatforms[Random.Range(0, _curentPlatforms.Length)];
                        pregen[i] = Ob;
                        now = Ob.GetComponent<Obstacle>();
                        
                    }
                    else
                    {
                        length = prev._GenerateDistance;

                    }
                    break;
                case OB_TYPE.COURSE:

                    length = prev._GenerateDistance;
                    break;
                case OB_TYPE.COIN:

                    length = prev._GenerateDistance;
                    break;
                case OB_TYPE.PLATFORM:
                    length = TwoOBDistantCheck(prev, now)+ prev._GenerateDistance;
                    if (now._passPoint == PASS_POINT.UP || now._passPoint == PASS_POINT.UP_MIDDLE)
                    {
                        length += _addLength;
                    }
                    break;



            }

            if(i>0 && genLength[i] == _jumpChaineLength && _jumpChaineLength == genLength[i - 1])
            {
                length += _addLength;
            }
            
            genLength[i] = length;
            pursegen[i] = pursePlace;
            coinRangeCount++;
            _OBcount++;
        }
        _pregenEmpty = false;


    }
    private void TimeKiper()
    {
        if (_transitioning)
        {
            switch (_nextScreen)
            {

                case SCREEN_ENUM.FOREST:
                    ForestTransitionAlt();
                    break;

                case SCREEN_ENUM.DESERT:
                    DesertTransition();
                    break;
                case SCREEN_ENUM.KRAKEN:
                    _transitioning = false;
                    StartCoroutine(KrakenTransition());
                    break;
            }
            
        }
      
    }
    private bool SpawnCheck()
    {
        if (_obstaclePause) return false;
        if (_LastObject != null)
        {
            float distans = _LastObject.transform.position.x;
            float genDistans = _LastObject.GetComponent<Obstacle>()._GenerateDistance;
            return (_spawnPoint.x - distans >= genDistans);
        }
        return true;

    }
    private bool SpawnCheckAlt()
    {
        if (_obstaclePause) return false;
        if (_pregenEmpty)
        {
            GenerateObAlt();
        }

        if (_LastObject != null)
        {
            float distans = _LastObject.transform.position.x;
            float genDistans = genLength[listCount];
            return (_spawnPoint.x - distans >= genDistans+_velocityLengthAdd);
        }

        return true;

    }

    private void SpawnOB()
    {
        GameObject Ob;
        Ob = Instantiate(pregen[listCount]);
        Ob.transform.position = new Vector2(_spawnPoint.x, _spawnPoint.y);
        _OBgenCount++;
        if (_coinChens <= Random.Range(1, 11) && Ob.GetComponent<Obstacle>() != null)
        {
            Ob.GetComponent<Obstacle>().ActivatePurse(pursegen[listCount]);
        }
        _LastObject = Ob;
        listCount++;
        if (pregen.Length <= listCount)
        {
            listCount = 0;
            _pregenEmpty = true;
        }
    }
    private float TwoOBDistantCheck(Obstacle OBa, Obstacle OBb)
    {
        float length = _minLength;
        switch (OBa._passPoint)
        {
            case Obstacle.PASS_POINT.UP:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _minLength + _addLength;
                        pursePlace =2;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        pursePlace =2;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        pursePlace =0;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        length = _minLength;
                        pursePlace =2;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _minLength;
                        pursePlace =2;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _addLength + _dropChaineLength;
                        pursePlace =2;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.MIDDLE:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _jumpChaineLength;
                        pursePlace =1;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        pursePlace =1;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        pursePlace =1;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        if (1 == Random.Range(0, 2)) { length = _jumpChaineLength; pursePlace = 1; }
                        else { length = _minLength; pursePlace = 2; }
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _jumpChaineLength;
                        pursePlace =1;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        if (1 == Random.Range(0, 2)) { length = _addLength; pursePlace = 1; }
                        else { length = _dropChaineLength; pursePlace = 2; }
                        break;


                }
                break;

            case Obstacle.PASS_POINT.DOWN:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _minLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _addLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        if (1 == Random.Range(0, 2)) length = _minLength;
                        else length = _minLength + _addLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:                        
                        length = _minLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _minLength;
                        pursePlace =2;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.UP_MIDDLE:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:

                        length = _jumpChaineLength;
                        pursePlace = 1;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        pursePlace = 1;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        pursePlace = 0;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        if (1 == Random.Range(0, 2)) { length = _minLength; pursePlace = 0; }
                        else { length = _jumpChaineLength; pursePlace =1; }
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        if (1 == Random.Range(0, 2)) { length = _dropChaineLength; pursePlace = 0; }
                        else { length = _jumpChaineLength; pursePlace =1; }

                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _dropChaineLength + _addLength;
                        pursePlace = 1;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.UP_DOWN:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _minLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        pursePlace = 0;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        length = _minLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _minLength;
                        pursePlace = 0;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _dropChaineLength;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.MIDDLE_DOWN:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        if (1 == Random.Range(0, 2)) { length = _jumpChaineLength; pursePlace = 1; }
                        else { length = _minLength; pursePlace =2; }
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        pursePlace = 2;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        pursePlace =1;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        length = _jumpChaineLength;
                        pursePlace = 1;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _jumpChaineLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _dropChaineLength;
                        break;


                }
                break;
        }
        return length;
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        OnEscapePressed?.Invoke();
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetObstacleToErea()
    {
        switch (_curentScreen)
        {

            case SCREEN_ENUM.FOREST:
                _curentObstecl = _forestObstecl;
                _curentObsteclCours = _forestObsteclCurse;
                _curentPlatforms = _forestPlatforms;
                break;

            case SCREEN_ENUM.DESERT:
                _curentObstecl = _desertObstecl;
                _curentObsteclCours = _desertObsteclCurse;
                _curentPlatforms = _desertPlatforms;
                break;
            default:
                _curentObstecl = _forestObstecl;
                _curentObsteclCours = _forestObsteclCurse;
                _curentPlatforms = _forestPlatforms;
                break;
        }
    }

    public static event Action OnChangeErea;
    public void InvokeCangeErea()
    {
        _nextScreen = (SCREEN_ENUM)(((int)_curentScreen + 1) % System.Enum.GetValues(typeof(SCREEN_ENUM)).Length);

        _obstaclePause = true;
        ClearAllObstacles?.Invoke();
        _transitioning = true;
        OnChangeErea?.Invoke();
        
        

    }
    private void CangeErea()
    {
        switch (_curentScreen)
        {
            case SCREEN_ENUM.FOREST:
                _forestBackground.SetActive(false);
                break;
            case SCREEN_ENUM.DESERT:
                _desertBackground.SetActive(false);
                break;
            case SCREEN_ENUM.KRAKEN:
                _krakenBackground.SetActive(false);
                break;

        }
        _curentScreen = _nextScreen;


        switch (_curentScreen)
        {
            case SCREEN_ENUM.FOREST:
                _forestBackground.SetActive(true);
                break;
            case SCREEN_ENUM.DESERT:
                _desertBackground.SetActive(true);
                break;
            case SCREEN_ENUM.KRAKEN:
                _krakenBackground.SetActive(true);
                break;

        }
        SetObstacleToErea();
        GenerateObAlt();
    }
    private void ForestTransition()
    {

        if (_transitionClock == 0)
        {

            _forestTransitionList[1].SetActive(true);
            _forestTransitionList[1].transform.position = _forestTransitionList[1].GetComponent<ereaBackgroundTransition>()._startLocation;

        }
        else if (_transitionClock >= _transitionSwitch && !_forestTransitionList[2].activeSelf)
        {

            _forestTransitionList[2].SetActive(true);
            CangeErea();


        }

        _transitionClock += Time.deltaTime;

        if (_transitionClock >= _transitionTime)
        {
            _forestTransitionList[3].SetActive(true);
            _forestTransitionList[3].transform.position = _forestTransitionList[3].GetComponent<ereaBackgroundTransition>()._startLocation;
            _forestTransitionList[2].SetActive(false);
            //_desertTransition_3.SetActive(true);
            _transitionClock = 0;
            _transitioning = false;
            _obstaclePause = false;
        }
    }
    private void DesertTransition()
    {

        if (_transitionClock == 0)
        { 

            _desertTransitionList[1].SetActive(true);
            _desertTransitionList[1].transform.position = _desertTransitionList[1].GetComponent<ereaBackgroundTransition>()._startLocation;

        }
        else if (_transitionClock >= _transitionSwitch && !_desertTransitionList[2].activeSelf)
        {

            _desertTransitionList[2].SetActive(true);
            CangeErea();


        }

        _transitionClock += Time.deltaTime;

        if (_transitionClock >= _transitionTime)
        {
            _desertTransitionList[3].SetActive(true);
            _desertTransitionList[3].transform.position = _desertTransitionList[3].GetComponent<ereaBackgroundTransition>()._startLocation;
            _desertTransitionList[2].SetActive(false);
            //_desertTransition_3.SetActive(true);
            
            _transitioning = false;
            _obstaclePause = false;
            _transitionClock = 0;
        }
    }

    private void AddVelocityObsteclDistant()
    {
        if (_Player.velocity.x > _velocityLengthAddThreshhold)
        {
            _velocityLengthAdd = (_Player.velocity.x)/10;
        }
    }
    public void InvokeClearOnScreenObstacles()
    {
        ClearOnScreenObstacles?.Invoke();
    }
    public void InvokeClearOffScreenObstacles()
    {
        ClearOffScreenObstacles?.Invoke();
    }
    IEnumerator KrakenTransition()
    {
        _forestTransitionList[1].SetActive(true);
        yield return new WaitForSeconds(_transitionSwitch);
        _forestTransitionList[2].SetActive(true);
        yield return new WaitForSeconds(3f);
        _Player.GetComponentInChildren<SpriteRenderer>().sortingOrder = 102;
        CollectablesManager.instance.DisableColectables();
        _krakenWaves[0].SetActive(true);
        //_krakenWaves[0].transform.localPosition = _waveSpawn[0];
        _krakenWaves[1].SetActive(true);
        //_krakenWaves[1].transform.localPosition = _waveSpawn[1];
        _krakenWaves[2].SetActive(true);
        //_krakenWaves[2].transform.localPosition = _waveSpawn[2];
        yield return new WaitForSeconds(3.5f);

        _krakenTentacleTranzition.SetActive(true);
        _krakenTentacleTranzition.transform.localPosition = _krakenTransitionSpawn;


        yield return new WaitForSeconds(8);

        _krakenTransition.SetActive(true);
        _krakenBubleTransition.SetActive(true);
        _krakenBubleTransition2.SetActive(true);
        _Player.transform.position = _playerBossSpawn;
        _Player._rigidbody.gravityScale = 0;
        for (int i = 0; i < 3; i++)
        {
            _krakenWaves[i].GetComponent<Parallax>()._yMove = true;
            _krakenWaves[i].GetComponent<Parallax>().depth = 1.5f;
            _krakenWaves[i].GetComponent<Parallax>().spawn = 4.5f;
            _krakenWaves[i].GetComponent<Parallax>().destroy = 100;
        }

        yield return new WaitForSeconds(4);
        CangeErea();
        _forestTransitionList[1].SetActive(false);
        _forestTransitionList[2].SetActive(false);
        _krakenTransition.GetComponentInParent<krakenBackgroundTransition>()._continu = true;
        for (int i = 0; i < 3; i++)
        {
            _krakenWaves[i].SetActive(false);
        }
        yield return new WaitForSeconds(2);
        _krakenBubleTransition.GetComponent<Parallax>().destroy = 500;
        _krakenBubleTransition2.GetComponent<Parallax>().destroy = 500;
        yield return new WaitForSeconds(1);
        _krakenTransition.SetActive(false);
       

        _Player.GetComponentInChildren<SpriteRenderer>().sortingOrder = 11;
        _Player._rigidbody.gravityScale = 10;
        yield return new WaitForSeconds(3);
        _krakenBubleTransition.SetActive(false);
        _krakenBubleTransition2.SetActive(false);
        _krakenBubleTransition.GetComponent<Parallax>().destroy = 50;
        _krakenBubleTransition2.GetComponent<Parallax>().destroy = 50;
        _kraken.transform.position = _krskenSpawn;
        _kraken.SetActive(true);
        _Player.CallPlayerHealth();
       
        yield return new WaitForSeconds(2);
       






    }
    int SW_forestTrans=0;
    private void ForestTransitionAlt()
    {
        Vector2 KrTrans = _krakenTransition.transform.position;
        switch (SW_forestTrans)
        {
            case 0:

                _krakenTransition.transform.position = new Vector2(KrTrans.x, 51);
                _krakenTransition.SetActive(true);
                _krakenTransition.GetComponent<krakenBackgroundTransition>().enabled = false;
                _krakenBubleTransition.SetActive(true);

                _krakenBubleTransition2.SetActive(true);
                SW_forestTrans++;
                break;

            case 1:
                KrTrans.y -= _krakenRetransitionSpeed * Time.deltaTime;
                _krakenTransition.transform.position = KrTrans;
                if(KrTrans.y <= 18.7 && KrTrans.y >= 18.6)
                {
                    _Player.transform.position = new Vector2 (20,3.6f);
                    CollectablesManager.instance.EnableColectables();
                    CangeErea();
                }
                if (KrTrans.y <= -14) SW_forestTrans++;
                break;
            case 2:
                _krakenTransition.SetActive(false);
                _krakenTransition.GetComponent<krakenBackgroundTransition>().enabled = true;
                _krakenBubleTransition.GetComponent<Parallax>()._willDisable = true;
                _krakenBubleTransition2.GetComponent<Parallax>()._willDisable = true;
                SW_forestTrans = 0;
                _transitioning = false;
                _obstaclePause = false;
                break;
        }
    }
    private void ControllOveride()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AnlishTheKraken();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            InvokeCangeErea();
        }
    }
    public void AnlishTheKraken()
    {
        _obstaclePause = true;
        ClearAllObstacles?.Invoke();
        _nextScreen = SCREEN_ENUM.KRAKEN;
        OnChangeErea?.Invoke();
        
        
        StartCoroutine(KrakenTransition());
    }
}
