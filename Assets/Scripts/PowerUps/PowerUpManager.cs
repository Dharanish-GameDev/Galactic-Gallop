using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance { get; private set; }

    public event Action DisableMeteorCollisForInvinciblity;
    public event Action EnableMeteorAfterInvinciblity;
    public event Action DisableInvinciblePowerUpPotion;

    [SerializeField] private List <SkinnedMeshRenderer> playerMeshRenderer;
    [SerializeField] private List <Material> playerMaterial;
    [SerializeField] private Material invincibleMaterial = null;
    [SerializeField] private float invinciblityTime;
    [SerializeField] private float sheildTime;

    private GameObject playerGameObject = null;
    private SkinnedMeshRenderer tempMeshRenderer = null;
    private PlayerStateContext playerStateContext = null;
    private SheildPowerUp sheildPowerUp = null;

    private bool hasPowerUp = false;
    private bool isPotionAlive;
    private int powerUpInt = 0; // -1 For Default, 0 for Invinciblity, 1 for Sheild.
    private readonly int SheildClose = Animator.StringToHash("SheildClose");

    // Properties
    public bool HasPowerUp { get { return hasPowerUp; } }
    public int PowerUpInt { get { return powerUpInt; } }
    public float InvinciblityTime { get { return invinciblityTime;} }
    public float SheildTime { get { return sheildTime;} }
    public PlayerStateContext PlayerStateContext { get { return playerStateContext; }}

    #region LifeCycle Methods

    private void Awake()
    {
        instance = this;
        isPotionAlive = false;
    }
    private void Start()
    {
        playerGameObject = PlayerManager.Instance.PlayerObject;
        playerStateContext = playerGameObject.GetComponent <PlayerStateContext>();
        sheildPowerUp = playerGameObject.GetComponentInChildren<SheildPowerUp>();
        hasPowerUp = false;
        
        StoringPlayerMats();
        SubcribingPowerUps();
        playerStateContext.PlayerEffects.DisableSheildVisual();
    }

    private void OnDisable()
    {
        UnSubscribingPowerUps();
    }

    /// <summary>
    /// 0 - DefaultMat , 1 - InvincibleMat
    /// </summary>
    /// <param name="i"></param>
    private void ChangePlayerMat(int matInt)
    {
        switch (matInt)
        {
            case 0:  // Default Mat
                for (int i = 0; i < playerMeshRenderer.Count; i++)
                {
                    playerMeshRenderer[i].material = playerMaterial[i];
                }
                break;

            case 1:  // Invincible Mat
                foreach (var player in playerMeshRenderer)
                {
                    player.material = invincibleMaterial;
                }
                break;
        }
    }

    #endregion

    #region Private Methods

    private void StoringPlayerMats()
    {
        for (int i = 0; i < playerGameObject.transform.childCount; i++)
        {
            tempMeshRenderer = null;
            playerGameObject.transform.GetChild(i).TryGetComponent<SkinnedMeshRenderer>(out tempMeshRenderer);
            if (tempMeshRenderer != null) playerMeshRenderer.Add(tempMeshRenderer);
        }
        foreach (SkinnedMeshRenderer playerMesh in playerMeshRenderer)
        {
            playerMaterial.Add(playerMesh.material);
        }
    }
    private void SubcribingPowerUps()
    {
        InvinciblePowerUp.OnInvinciblePowerCaught += InvinciblePowerUp_OnInvinciblePowerCaught;
        DoubleTap.OnDoubleTap += OnDoubleTapped;
        playerStateContext.OnHittingObstableWidSheild += OnHittingObstableWidSheild;
    }

    private void UnSubscribingPowerUps()
    {
        InvinciblePowerUp.OnInvinciblePowerCaught -= InvinciblePowerUp_OnInvinciblePowerCaught;
        DoubleTap.OnDoubleTap -= OnDoubleTapped;
        playerStateContext.OnHittingObstableWidSheild -= OnHittingObstableWidSheild;
    }

    private void OnDoubleTapped()
    {
        if(hasPowerUp)
        {
            DisableInvinciblePowerUpPotion?.Invoke();
            return;
        }
        if (PlayerPrefs.GetInt("SheildCount") == 0) return;
        SetPowerUp(1); // For For Sheild
        playerStateContext.PlayerEffects.EnableSheildVisual();
        PlayerPrefs.SetInt("SheildCount", PlayerPrefs.GetInt("SheildCount") - 1);
        UI_Manager.Instance.UpdateSheildCountText();
        UI_Manager.Instance.ActivatePowerUpSlider(true);
        Invoke(nameof(PlaySheildCloseAnimation), sheildTime);
        DisableInvinciblePowerUpPotion?.Invoke();
        MakePotionAliveFalse();
        AudioManager.instance.PlaySfx(AudioManager.instance.AudioClips.gotPowerupSfx, AudioManager.instance.AudioSourcesList.powerUpAudioSource, 1);
    }

    // PowerUp Methods
    private void InvinciblePowerUp_OnInvinciblePowerCaught()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.AudioClips.gotPowerupSfx, AudioManager.instance.AudioSourcesList.powerUpAudioSource, 1);
        ChangePlayerMat(1);
        DisableMeteorCollisForInvinciblity?.Invoke();
        SetPowerUp(0); // 0 For Invinciblity
        UI_Manager.Instance.ActivatePowerUpSlider(true);
        Invoke(nameof(DisableInvinciblity), invinciblityTime);
        StartCoroutine(nameof(BlinkPlayer));
    }

    private void DisableInvinciblity()
    {
        ChangePlayerMat(0);
        UI_Manager.Instance.ActivatePowerUpSlider(false);
        EnableMeteorAfterInvinciblity?.Invoke();
        ResetPowerUp();
    }

    private IEnumerator BlinkPlayer()
    {
        if (!hasPowerUp) yield return null;
        yield return new WaitForSeconds(4.2f);
        ChangePlayerMat(0);
        yield return new WaitForSeconds(0.2f);
        ChangePlayerMat(1);
        yield return new WaitForSeconds(0.2f);
        ChangePlayerMat(0);
        yield return new WaitForSeconds(0.2f);
        ChangePlayerMat(1);
        MakePotionAliveFalse();
    }
    
    private void SetPowerUp(int powerInt)
    {
        hasPowerUp = true;
        powerUpInt = powerInt;
    }
    private void ResetPowerUp()
    {
        hasPowerUp = false;
        powerUpInt = -1;
    }
    private void OnHittingObstableWidSheild()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.AudioClips.sheildBreakSfx, AudioManager.instance.AudioSourcesList.powerUpAudioSource, 1);
        playerStateContext.PlayerEffects.DisableSheildVisual();
        ResetPowerUp();
        playerStateContext.PlayerEffects.PlaySheildBreakEfx();
        UI_Manager.Instance.ActivatePowerUpSlider(false);
    }
    private void PlaySheildCloseAnimation()
    {
        if (!hasPowerUp) return;
        playerStateContext.PlayerEffects.PlayWaterSplashEfx();
        playerStateContext.PlayerEffects.TriggerSheildCloseAni();
    }

    #endregion

    #region Public Methods

    public void DisableSheildAfterTime() // This methods is called as an Animation Event.
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.AudioClips.sheildBreakSfx, AudioManager.instance.AudioSourcesList.powerUpAudioSource, 1);
        playerStateContext.PlayerEffects.DisableSheildVisual();
        ResetPowerUp();
        UI_Manager.Instance.ActivatePowerUpSlider(false);
    }
    public void MakePotionAliveTrue()
    {
        isPotionAlive = true;
    }
    public void MakePotionAliveFalse()
    {
        isPotionAlive = false;
    }
    public bool CanSpawnPotion()
    {
        return !isPotionAlive;
    }

    #endregion
}
