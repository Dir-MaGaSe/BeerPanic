using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "BeerPanic/Elements/Base")]
public class ElementBase : ScriptableObject
{
    [Header("Configuración Básica")]
    public string elementName;
    public Sprite elementSprite;
    public AudioClip elementAudio;
    [Range(0f, 1f)] public float spawnProbability = 0.5f;
    [Range(.1f, 6f)] public float fallSpeed = 3f;
    
    [Header("Puntuación")]
    public int basePoints = 100;
}