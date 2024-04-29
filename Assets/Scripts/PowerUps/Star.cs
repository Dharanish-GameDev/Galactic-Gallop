using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.AddScore();
            PowerUpManager.instance.PlayerStateContext.PlayerEffects.PlayStarCollectingEFX();
            if(!PlayerManager.gameOver)
            {
                AudioManager.instance.PlaySfx(AudioManager.instance.AudioClips.starCollectingSfx, AudioManager.instance.AudioSourcesList.collectingSfxSource, 0.5f);
            }
            gameObject.SetActive(false);
        }
    }
}
