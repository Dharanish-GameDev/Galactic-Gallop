using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{
    private Vector3 downVector;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider boxCollider;
    public bool canFall = false;

    private void Start()
    {
        //PowerUpManager.instance.DisableMeteorCollisForInvinciblity += DisablingColli;
        //PowerUpManager.instance.EnableMeteorAfterInvinciblity += EnableColli;
    }

    private void Update()
    { 
        downVector.y = Time.deltaTime;
        if(transform.position.y > 0.6f && canFall)
        {
            transform.position -= downVector * speedMultiplier;
        }
        else if(transform.position.y <= 0.6f)
        {
            animator.enabled = true;
            Invoke(nameof(SelfDestruct), 0.5f);
            if (PlayerManager.gameOver) return;
            AudioManager.instance.PlaySfxOneTime(AudioManager.instance.AudioClips.meteorSFX,AudioManager.instance.AudioSourcesList.meteorBerakSfxSource,1);
        }
    }

    private void SelfDestruct()
    {
        gameObject.SetActive(false);
    }
}
