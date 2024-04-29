using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] Characters;
    [SerializeField] GameObject[] Models;
    [SerializeField] int CurrentCharacter;
    public Char_Blueprint[] bluePrint;
    [SerializeField] Button BuyButton;
    [SerializeField] int StarCount;
    [SerializeField] TextMeshProUGUI BurgerCountText;
    [SerializeField] TextMeshProUGUI BurgerCountTextOpti;
    [SerializeField] TextMeshProUGUI SelectedCharText;
    [SerializeField] private TextMeshProUGUI sheildCountTxt;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject buyCanvas;
    [SerializeField] private GameObject optionCanvas;
    [SerializeField] private GameObject charactersHolder;
    [SerializeField] private Slider volumeSlider;
    void Start()
    {
        buyCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        charactersHolder.SetActive(false);
        int tempNum = PlayerPrefs.GetInt("SelectedChar");
        for (int i = 0; i < Models.Length; i++)
        {
            Models[i].gameObject.SetActive(false);
            CurrentCharacter = tempNum;
            Models[CurrentCharacter].gameObject.SetActive(true);
        }
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i].gameObject.SetActive(false);
            CurrentCharacter = tempNum;
            Characters[CurrentCharacter].gameObject.SetActive(true);
        }
        Time.timeScale = 1;
        StarCount = PlayerPrefs.GetInt("StarCount");
        BurgerCountText.text = "Stars : " + StarCount;
        BurgerCountTextOpti.text = "Stars : " + StarCount;
        foreach (Char_Blueprint charac in bluePrint)
        {
            if (charac.price == 0)
            {
                charac.IsUnlocked = true;
            }
            else
            {
                charac.IsUnlocked = PlayerPrefs.GetInt(charac.name, 0) == 0 ? false : true;
            }
        }
        SelectedCharTextUpdate();
        sheildCountTxt.SetText(PlayerPrefs.GetInt("SheildCount").ToString());
        volumeSlider.value = 1;
        AudioListener.volume = 1;
        volumeSlider.onValueChanged.AddListener((value)=>{AudioListener.volume = value;});
    }

    void Update()
    {
        UpdateUI();
        
    }

    private void SelectedCharTextUpdate()
    {
        switch (CurrentCharacter)
        {
            case 0:
                SelectedCharText.text = "AGENT : JIMMY";
                break;
            case 1:
                SelectedCharText.text = "AGENT : CLARIE";
                break;
            case 2:
                SelectedCharText.text = "AGENT : BOSS";
                break;
            case 3:
                SelectedCharText.text = "AGENT : ZOMBIE";
                break;
            default:
                SelectedCharText.text = "AGENT : JIMMY";
                break;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }
    public void CloseGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
    public void LeftButton()
    {
        if(CurrentCharacter>=0)
        {
            CurrentCharacter -= 1;
            if(CurrentCharacter<0)
            {
                CurrentCharacter = 3;
            }
            for(int i =0;i<Characters.Length;i++)
            {
                Characters[i].gameObject.SetActive(false);
                Models[i].gameObject.SetActive(false);
                Characters[CurrentCharacter].gameObject.SetActive(true);
                Models[CurrentCharacter].gameObject.SetActive(true);
            }
        }
        Char_Blueprint charac = bluePrint[CurrentCharacter];
        if (!charac.IsUnlocked)
            return;
        SelectedCharTextUpdate();
        PlayerPrefs.SetInt("SelectedChar", CurrentCharacter);
    }
    public void RightButton()
    {
        if(CurrentCharacter <= Characters.Length - 1)
        {
            CurrentCharacter += 1;
            if(CurrentCharacter>3)
            {
                CurrentCharacter = 0;
            }
            for(int i =0;i< Characters.Length;i++)
            {
                Characters[i].gameObject.SetActive(false);
                Models[i].gameObject.SetActive(false);
                Characters[CurrentCharacter].gameObject.SetActive(true);
                Models[CurrentCharacter].gameObject.SetActive(true);
            }
            Char_Blueprint charac = bluePrint[CurrentCharacter];
            if (!charac.IsUnlocked)
                return;
            SelectedCharTextUpdate();
            PlayerPrefs.SetInt("SelectedChar", CurrentCharacter);
        }
    }
    public void BuyChar()
    {
        Char_Blueprint charac = bluePrint[CurrentCharacter];
        PlayerPrefs.SetInt(charac.name, 1);
        PlayerPrefs.SetInt("SelectedChar", CurrentCharacter);
        charac.IsUnlocked = true;
        PlayerPrefs.SetInt("StarCount", PlayerPrefs.GetInt("StarCount", 0) - charac.price);
    }
    public void BuySheild()
    {
        if (PlayerPrefs.GetInt("StarCount") < 50) return;
        PlayerPrefs.SetInt("SheildCount",PlayerPrefs.GetInt("SheildCount")+1);
        PlayerPrefs.SetInt("StarCount", PlayerPrefs.GetInt("StarCount", 0) - 50);
        sheildCountTxt.SetText(PlayerPrefs.GetInt("SheildCount").ToString());
    }
    private void UpdateUI()
    {
        StarCount = PlayerPrefs.GetInt("StarCount");
        BurgerCountText.text = "Stars : " + StarCount;
        BurgerCountTextOpti.text = "Stars : " + StarCount;
        Char_Blueprint charac = bluePrint[CurrentCharacter];
        if(charac.IsUnlocked)
        {
            BuyButton.gameObject.SetActive(false);
        }
        else
        {
            BuyButton.gameObject.SetActive(true);
            BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = charac.price.ToString();
            if(charac.price <= PlayerPrefs.GetInt("StarCount",0))
            {
                BuyButton.interactable = true;
            }
            else BuyButton.interactable = false;
        }
        
    }
    public void ClearPlayersPrefs()
    {
        PlayerPrefs.DeleteAll();
        foreach (Char_Blueprint char_Blueprint in bluePrint)
        {
            if(char_Blueprint.price!=0) 
            {
                char_Blueprint.IsUnlocked = false;
            }  
        }
        sheildCountTxt.SetText(PlayerPrefs.GetInt("SheildCount").ToString());
    }

    public void LoadBuy()
    {
        mainMenuCanvas.SetActive(false);
        buyCanvas.SetActive(true);
        charactersHolder.SetActive(true);
    }
    public void LoadOptions()
    {
        optionCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    public void ReturnToMain()
    {
        buyCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        charactersHolder.SetActive(false);
    }
}
