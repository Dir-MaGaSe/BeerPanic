using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(AudioManager.Instance == null)
        {
            AudioManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = GetComponent<AudioSource>();
        sfxSource = GetComponent<AudioSource>();
    }
    
    private void OnEnable() { musicSource.Play(); }

    public void PlayEffect(AudioClip soundEffect){ sfxSource.PlayOneShot(soundEffect); }
}
