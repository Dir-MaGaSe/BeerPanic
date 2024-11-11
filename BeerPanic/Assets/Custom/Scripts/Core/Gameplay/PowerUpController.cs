using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PowerUpController : MonoBehaviour
{
    private Dictionary<PowerUpElement.PowerUpType, float> activeEffects = 
        new Dictionary<PowerUpElement.PowerUpType, float>();
    
    private PlayerController playerController;
    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        UpdateEffects();
    }
    
    public void ApplyPowerUp(PowerUpElement powerUp)
    {
        if (!activeEffects.ContainsKey(powerUp.powerUpType))
        {
            switch (powerUp.powerUpType)
            {
                case PowerUpElement.PowerUpType.Speed:
                    playerController.ApplySpeedModifier(powerUp.effectMultiplier, powerUp.effectDuration);
                    break;
                case PowerUpElement.PowerUpType.CatchArea:
                    playerController.ApplyCatchAreaModifier(powerUp.effectMultiplier, powerUp.effectDuration);
                    break;
            }
            
            activeEffects[powerUp.powerUpType] = Time.time + powerUp.effectDuration;
        }
    }
    
    private void UpdateEffects()
    {
        List<PowerUpElement.PowerUpType> expiredEffects = new List<PowerUpElement.PowerUpType>();
        
        foreach (var effect in activeEffects)
        {
            if (Time.time >= effect.Value)
            {
                expiredEffects.Add(effect.Key);
            }
        }
        
        foreach (var effect in expiredEffects)
        {
            activeEffects.Remove(effect);
        }
    }
}