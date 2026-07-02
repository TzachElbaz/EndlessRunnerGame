using UnityEngine;
using static RunGameManeger;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SfxSource;

    [Header("Audio Clip")]
    public AudioClip menu;
    public AudioClip forest;
    public AudioClip desert;
    public AudioClip kraken;
    public AudioClip bubbles;
    public AudioClip coin;
    public AudioClip jump;
    //public AudioClip doubleJump;
    public AudioClip hit;
    public AudioClip collect;


    private void Start()
    {
        musicSource.clip = forest;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void OnEnable()
    {
        RunGameManeger.OnEscapePressed += PouseMusic;
        Player.OnPlayerDied += PlayMenuMusic;
        RunGameManeger.OnChangeErea += OnchangeErea;
    }
    private void OnDisable()
    {
        RunGameManeger.OnEscapePressed -= PouseMusic;
        Player.OnPlayerDied -= PlayMenuMusic;
        RunGameManeger.OnChangeErea -= OnchangeErea;

    }

    public void PlaySFX(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }

    private void PouseMusic()
    {
        if (RunGameManeger.isGamePaused) musicSource.Pause();
        else musicSource.UnPause();
    }

    private void PlayMenuMusic()
    {
        musicSource.clip = menu;
        musicSource.Play();
    }

    private void OnchangeErea()
    {
        var nextScreen = RunGameManeger.Instance._nextScreen;

        musicSource.clip = nextScreen switch
        {
            SCREEN_ENUM.FOREST => forest,
            SCREEN_ENUM.DESERT => desert,
            //SCREEN_ENUM.KRAKEN => kraken,
            _ => forest
        };
        musicSource.Play();
    }

}
