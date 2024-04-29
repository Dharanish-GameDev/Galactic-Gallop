using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem sheildBreakParticleSystem;
    [SerializeField] private ParticleSystem waterSplashParticle;
    [SerializeField] private ParticleSystem potionCaughtParticle;
    [SerializeField] private ParticleSystem starCollectingparticle;
    [SerializeField] private GameObject sheildObject;
    [SerializeField] private GameObject slideParticleObject;
    [SerializeField] private GameObject jumpParticleObject;
    [SerializeField] private GameObject spotLight;

    private Animator sheildAnimator;
    private readonly int SheildClose = Animator.StringToHash("SheildClose");

    private void Awake()
    {
        DisableSheildVisual();
        DisableSlideParticle();
        DisableJumpParticle();
        spotLight.SetActive(true);
        sheildAnimator = sheildObject.GetComponent<Animator>();
    }

    #region Public Methods

    public void EnableSheildVisual()
    {
        sheildObject.SetActive(true);
    }
    public void DisableSheildVisual()
    {
        sheildObject.SetActive(false);
    }
    public void EnableSlideParticle()
    {
        slideParticleObject.SetActive(true);
    }
    public void DisableSlideParticle()
    {
        slideParticleObject.SetActive(false);
    }
    public void EnableJumpParticle()
    {
        jumpParticleObject.SetActive(true);
    }
    public void DisableJumpParticle()
    {
        jumpParticleObject.SetActive(false);
    }
    public void TriggerSheildCloseAni()
    {
        sheildAnimator.SetTrigger(SheildClose);
    }
    public void PlaySheildBreakEfx()
    {
        sheildBreakParticleSystem.Play();
    }
    public void PlayWaterSplashEfx()
    {
        waterSplashParticle.Play();
    }
    public void PlayPotionCaughtEfx()
    {
        potionCaughtParticle.Play();
    }
    public void DisableSpotLight()
    {
        spotLight.SetActive(false);
    }
    public void PlayStarCollectingEFX()
    {
        starCollectingparticle.Play();
    }
    #endregion
}
