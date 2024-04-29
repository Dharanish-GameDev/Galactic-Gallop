using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLine : MonoBehaviour
{

    [SerializeField] private GameObject potionGameObject;
    [SerializeField] private List <Collider> obstacleCollis;

    private void Start()
    {
        PowerUpManager_DisableInvinciblePowerUpPotion();
        if(potionGameObject)
        {
            PowerUpManager.instance.DisableInvinciblePowerUpPotion += PowerUpManager_DisableInvinciblePowerUpPotion;
            if(PowerUpManager.instance.CanSpawnPotion())
            {
                float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                if (rand > 0.1f && rand < 0.25f)
                {
                    potionGameObject.SetActive(true);
                    PowerUpManager.instance.MakePotionAliveTrue();
                }
            }
        }
        PowerUpManager.instance.DisableMeteorCollisForInvinciblity += DisableMeteorCollisForInvinciblity;
        PowerUpManager.instance.EnableMeteorAfterInvinciblity += EnableMeteorAfterInvinciblity;
    }

    private void EnableMeteorAfterInvinciblity()
    {
        if (obstacleCollis.Count > 0)
        {
            foreach (Collider col in obstacleCollis)
            {
                if(col!= null)
                    col.enabled = true;
            }
        }
    }

    private void DisableMeteorCollisForInvinciblity()
    {
        if(obstacleCollis.Count > 0)
        {
            foreach(Collider col in obstacleCollis)
            {
                if(col!= null)
                    col.enabled = false;
            }
        }
    }

    private void PowerUpManager_DisableInvinciblePowerUpPotion()
    {
        if(potionGameObject!= null)
        {
            potionGameObject.SetActive(false);
        }
    }
}
