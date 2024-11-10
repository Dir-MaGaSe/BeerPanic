using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "BeerPanic/Elements/PowerUp")]
public class PowerUpElement : ElementBase
{
    [Header("Configuración de Potenciador")]
    public float effectDuration = 5f;
    public float effectMultiplier = 1.5f;
    public PowerUpType powerUpType;
    
    public enum PowerUpType
    {
        Speed,
        CatchArea
    }
}