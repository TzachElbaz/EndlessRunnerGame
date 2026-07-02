using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private AudioManager _audioManager;

    [SerializeField] private int _coinValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CollectablesManager collectables;
    private void Awake()
    {
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _audioManager.PlaySFX(_audioManager.coin);
            collectables._coinCount += _coinValue;
        }
        Destroy(gameObject);

    }
}
