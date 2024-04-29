using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }


    [SerializeField] private GameObject powerUpSliderObject;
    [SerializeField] private Image leftSlider;
    [SerializeField] private Image rightSlider;

    [Header("Slider Sprites")]
    [SerializeField] private Sprite invincibleSliderSprite;
    [SerializeField] private Sprite sheildSliderSprite;

    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private GameObject countDownTextObejct;


    public static int PreviousStarCount;

    private float SliderWaitTime;
    private float fillAmount;

    [Header("PowerUp")]
    [SerializeField] private TextMeshProUGUI sheildCountText;

    #region LifeCycle Methods
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if(PlayerPrefs.GetInt("StarCount")!=0)
        {
            PreviousStarCount = PlayerPrefs.GetInt("StarCount");
        }
        else
        {
            PreviousStarCount = 0;
        }
        powerUpSliderObject.SetActive(false);
        countDownTextObejct.SetActive(false);
        UpdateSheildCountText();
        resumeBtn.onClick.AddListener(() =>
        {
            pausePanel.gameObject.SetActive(false);
            countDownTextObejct.SetActive(true);
        });
        pauseButton.onClick.AddListener(() => { 
            Time.timeScale = 0f; 
            pausePanel.SetActive(true);
        });
    }
    private void Update()
    {
        if (!PowerUpManager.instance.HasPowerUp) return;
        UpdatePowerUpSlider();
    }

    #endregion

    #region Private Methods

    private void UpdatePowerUpSlider()
    {
        if (SliderWaitTime > 0)
        {
            SliderWaitTime -= Time.deltaTime;
            fillAmount = Mathf.InverseLerp(0, PowerUpManager.instance.InvinciblityTime, SliderWaitTime);
            SetPowerSliderFillValue(fillAmount);
        }
        
    }

    #endregion

    #region Public Methods
    public void Restart()
    {
        PlayerPrefs.SetInt("StarCount", PlayerManager.Instance.NoOfStars + PreviousStarCount);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2);
    }
    public void Quit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
    public void MainMenu()
    {
        PlayerPrefs.SetInt("StarCount",PlayerManager.Instance.NoOfStars + PreviousStarCount);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void ActivatePowerUpSlider(bool activity)
    {
        powerUpSliderObject.SetActive(activity);
        leftSlider.fillAmount = activity ? 1 : 0;
        rightSlider.fillAmount = activity ? 1 : 0;
        switch (PowerUpManager.instance.PowerUpInt)
        {
            case 0:
                SliderWaitTime = PowerUpManager.instance.InvinciblityTime;
                leftSlider.sprite = invincibleSliderSprite;
                rightSlider.sprite = invincibleSliderSprite;

                break;

                case 1:
                SliderWaitTime = PowerUpManager.instance.SheildTime;
                leftSlider.sprite = sheildSliderSprite;
                rightSlider.sprite = sheildSliderSprite;
                break;
        }

        
    }
    public void SetPowerSliderFillValue(float value)
    {
        leftSlider.fillAmount =  value;
        rightSlider.fillAmount = value;
    }
    public void UpdateSheildCountText()
    {
        sheildCountText.text = "x " + PlayerPrefs.GetInt("SheildCount");
    }

    #endregion
    
}
