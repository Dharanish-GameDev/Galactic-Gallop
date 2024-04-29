using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePowerUp : MonoBehaviour
{

    public static event Action OnInvinciblePowerCaught;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnInvinciblePowerCaught?.Invoke();
            PowerUpManager.instance.PlayerStateContext.PlayerEffects.PlayPotionCaughtEfx();
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        PowerUpManager.instance.MakePotionAliveTrue();
    }
    private void OnDestroy()
    {
       PowerUpManager.instance.MakePotionAliveFalse();
    }
}
