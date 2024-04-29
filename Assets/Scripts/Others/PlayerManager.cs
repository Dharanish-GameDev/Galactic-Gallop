using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class PlayerManager : MonoBehaviour
{
    //Singleton
    public static PlayerManager Instance { get; private set; }

    public enum GameState
    {
        Counting,
        Playing,
        GameOver
    }

    public static bool gameOver;
    public GameObject GameOverpanel;
    public static bool IsgameStarted = false;
    [HideInInspector]
    public int NoOfStars;
    public GameState gameState;

    
    [SerializeField] private GameObject StartingText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject[] Characters;


    private readonly int IsGameStarted = Animator.StringToHash("IsGameStarted");
    private GameObject playerObject = null;
    private Animator animator;

    #region Properties

    public GameObject PlayerObject { get { return playerObject; } }

    #endregion

    #region LifeCycle Methods
    private void Awake()
    {
        Instance = this;
        DisableAllCharacters();
        EnablePlayableCharacter();
    }

    void Start()
    {
        NoOfStars = 0;
        ScoreText.text = "Stars : " + NoOfStars;
        IsgameStarted = false;
        Time.timeScale = 1f;
        gameOver = false;
        gameState = GameState.Counting;
    }
    void Update()
    {

        switch(gameState)
        {
            case GameState.Counting:
                if (Swipe_Deduction.tap)
                {
                    gameState= GameState.Playing;
                    IsgameStarted = true;
                    animator.SetBool(IsGameStarted, true);
                    if (StartingText == null) return;
                    Destroy(StartingText);
                    PowerUpManager.instance.PlayerStateContext.PlayerEffects.DisableSpotLight();
                }
                break;
            case GameState.Playing:

                break;
        }
        if(gameOver)
        {
            Time.timeScale = 0f;
            GameOverpanel.SetActive(true);
        }
        
    }

    #endregion
    #region Public Methods
    public void AddScore()
    {
        NoOfStars++;

        ScoreText.text = "Score : " + NoOfStars;
    }
    #endregion

    #region Private Methods
    private void DisableAllCharacters()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i].SetActive(false);
        }
    }
    private void EnablePlayableCharacter()
    {
        Characters[PlayerPrefs.GetInt("SelectedChar")].SetActive(true);
        playerObject = Characters[PlayerPrefs.GetInt("SelectedChar")]; // Assinging The Player Character
        animator = playerObject.GetComponent<Animator>();
    }
    #endregion

}
