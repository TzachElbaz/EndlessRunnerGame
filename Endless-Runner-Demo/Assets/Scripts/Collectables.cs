using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    private AudioManager _audioManager;
    CollectablesManager collectables;
    RunGameManeger _runGameManager;
    [SerializeField, HideInInspector] private int _colectableId;   
    [SerializeField] SO_Collectable _so;
    [SerializeField, HideInInspector] public SO_Collectable.SCREEN_COL _zone;
    [SerializeField, HideInInspector] private Sprite _sprite;
    [SerializeField, HideInInspector] public Animator _animation;
    [SerializeField, HideInInspector] public string _name;


    private void Awake()
    {
        _animation = GetComponentInChildren<Animator>();
        _runGameManager = GameObject.FindAnyObjectByType<RunGameManeger>();
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (!collectables._isCollectableAvalable) Destroy(gameObject);

        int random = 0;
        while (collectables._colectableList[random] && random < collectables._colectableList.Length) random++;
        switch (_runGameManager._curentScreen)
        {
            case RunGameManeger.SCREEN_ENUM.FOREST:
                _so = collectables._soForestCollectableList[random];
                break;
            case RunGameManeger.SCREEN_ENUM.DESERT:
                _so = collectables._soDesertCollectableList[random];
                break;
        }
        
        _colectableId = _so._colectableId;
        _zone = _so._zone;
        _sprite = _so._sprite;
        _name = _so._name;
        _animation.Play(_name);
        

        collectables._isCollectableAvalable = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        collectables.SetCollectable(_colectableId);
        _audioManager.PlaySFX(_audioManager.collect);
        Destroy(gameObject);
    }
    
}
