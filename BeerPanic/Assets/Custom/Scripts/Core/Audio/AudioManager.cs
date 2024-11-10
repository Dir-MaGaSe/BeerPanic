using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [System.Serializable]
    public class SoundEffect
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        public bool usePooling = true;  // Para efectos que necesitan pooling
    }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;
    
    [Header("Sound Effects")]
    [SerializeField] private SoundEffect[] soundEffects;
    
    private Dictionary<string, SoundEffect> effectDictionary;
    private ObjectPool<AudioSource> audioSourcePool;
    private AudioSettings audioSettings;
    private bool isInitialized;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Initialize(AudioSettings settings)
    {
        audioSettings = settings;
        ApplyAudioSettings();
    }
    
    private void InitializeAudioSystem()
    {
        effectDictionary = new Dictionary<string, SoundEffect>();
        foreach (var effect in soundEffects)
        {
            effectDictionary[effect.id] = effect;
        }
        
        //audioSourcePool = new ObjectPool<AudioSource>(CreateAudioSource, 5);
        isInitialized = true;
    }
    
    private void ApplyAudioSettings()
    {
        if (audioSettings != null)
        {
            musicSource.volume = audioSettings.musicVolume;
            effectsSource.volume = audioSettings.effectsVolume;
        }
    }
    
    private AudioSource CreateAudioSource()
    {
        GameObject obj = new GameObject("Pooled Audio Source");
        obj.transform.SetParent(transform);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }
    
    public void PlayMusic(AudioClip music, bool loop = true)
    {
        if (!isInitialized) return;
        
        musicSource.clip = music;
        musicSource.loop = loop;
        musicSource.Play();
    }
    
    public void PlayEffect(string effectId)
    {
        if (!isInitialized) return;
        
        if (effectDictionary.TryGetValue(effectId, out SoundEffect effect))
        {
            if (effect.usePooling)
            {
                AudioSource source = audioSourcePool.Get();
                source.clip = effect.clip;
                source.volume = effect.volume * audioSettings.effectsVolume;
                source.Play();
                
                StartCoroutine(ReturnToPoolAfterPlay(source));
            }
            else
            {
                effectsSource.PlayOneShot(effect.clip, effect.volume * audioSettings.effectsVolume);
            }
        }
    }
    
    private System.Collections.IEnumerator ReturnToPoolAfterPlay(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        audioSourcePool.Return(source);
    }
    
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    
    public void SetEffectsVolume(float volume)
    {
        effectsSource.volume = volume;
    }
}