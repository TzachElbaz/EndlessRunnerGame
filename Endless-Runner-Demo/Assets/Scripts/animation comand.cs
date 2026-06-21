using JetBrains.Annotations;
using UnityEngine;

public class animationcomand : MonoBehaviour
{
    
    private Player player;
    private RunGameManeger runGameManeger;
    private CollectablesManager collectablesManager;
    private BossScript kraken;
    public bool _PlayerGrab;
    public bool _DestoyMe;
    public bool _DisableMe;
    public bool _PlayerBossTeleport;
    //public Vector2 _teleport;
    //public Vector2 _teleportPerent;

    void Start()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        runGameManeger = GameObject.FindAnyObjectByType<RunGameManeger>();
        collectablesManager = GameObject.FindAnyObjectByType<CollectablesManager>();
        kraken = GameObject.FindAnyObjectByType<BossScript>();
    }

    
    void Update()
    {
        if (_PlayerGrab) PlayerGrab();
        if (_DestoyMe) DestoyMe();
        if (_DisableMe) DisableMe();
        if (_PlayerBossTeleport) PlayerBossTeleport();
       
        //if (_teleport != null) Teleport();
        //if (_teleportPerent != null) TeleportPerent();
    }

    public void PlayerGrab()
    {
        Debug.Log("bip");
        
        player.TentacleGrabAnimation();
        _PlayerGrab = false;
    }
    public void DestoyMe()
    {
        
        Destroy(gameObject);
        _DestoyMe = false;

    }
    public void DisableMe()
    {

        gameObject.SetActive(false);
        _DisableMe = false;
    }

    public void PlayerBossTeleport()
    {

        _PlayerBossTeleport = false;
    }
    public void SpawnBulshit()
    {
        kraken.SpawnBulshit();
    }
    public void ChooseBulshit() 
    {
        kraken.ChooseBulshit();
    }

    //public void Teleport()
    //{

    //       transform.position = _teleport;

    //}
    //public void TeleportPerent()
    //{

    //        GameObject parent = transform.parent.gameObject;
    //        parent.transform.position = _teleport;

    //}
}
