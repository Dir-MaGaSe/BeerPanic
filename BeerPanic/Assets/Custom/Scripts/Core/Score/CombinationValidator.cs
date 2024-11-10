using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinationValidator
{
    private readonly Dictionary<string, List<string>> validCombinations;

    public CombinationValidator()
    {
        validCombinations = new Dictionary<string, List<string>>();
    }

    public void RegisterCombination(string fruitId, List<string> compatibleFruits)
    {
        if (!validCombinations.ContainsKey(fruitId))
        {
            validCombinations[fruitId] = compatibleFruits;
        }
    }

    public bool ValidateCombination(List<string> fruitIds)
    {
        if (fruitIds.Count < 2) return false;

        string firstFruit = fruitIds[0];
        if (!validCombinations.ContainsKey(firstFruit)) return false;

        return fruitIds.Skip(1).All(fruitId => validCombinations[firstFruit].Contains(fruitId));
    }
}