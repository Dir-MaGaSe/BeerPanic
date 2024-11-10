using UnityEngine;

[CreateAssetMenu(fileName = "NewObstacle", menuName = "BeerPanic/Elements/Obstacle")]
public class ObstacleElement : ElementBase
{
    [Header("Configuración de Obstáculo")]
    public float effectDuration = 3f, penaltyMultiplier = 0.5f;
}