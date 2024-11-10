using UnityEngine;

[CreateAssetMenu(fileName = "NewFruit", menuName = "BeerPanic/Elements/Fruit")]
public class FruitElement : ElementBase
{
    [Header("Configuración de Fruta")]
    public string[] validCombinations; // IDs de otras frutas para combinaciones
    public int combinationMultiplier = 2;
}