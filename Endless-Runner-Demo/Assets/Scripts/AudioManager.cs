using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SfxSource;

    [Header("Audio Clip")]
    public AudioClip menu;
    public AudioClip forest;
    public AudioClip desert;
    public AudioClip atlantis;
    public AudioClip bubbles;
    public AudioClip coin;
    public AudioClip jump;
    public AudioClip doubleJump;
    public AudioClip hit;
    public AudioClip Groundhit;


    private void Start()
    {
        musicSource.clip = forest;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }

}
