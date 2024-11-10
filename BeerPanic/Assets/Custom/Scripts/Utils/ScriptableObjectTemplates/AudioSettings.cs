using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioConfig", menuName = "BeerPanic/Settings/AudioSettings")]
public class AudioSettings : ScriptableObject
{   
    [Header("Configuración de Volumen")]
    public float musicVolume;
    public float effectsVolume;
}