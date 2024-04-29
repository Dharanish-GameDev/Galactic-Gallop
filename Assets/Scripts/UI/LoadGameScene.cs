using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    private void Start()
    {
        StartCoroutine(LoadingText());
        Time.timeScale = 1.0f;
        Invoke(nameof(LoadLevel),0.65f);
    }
    private void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }
    private IEnumerator LoadingText()
    {
        loadingText.text = "Loading";
        yield return new WaitForSeconds(0.2f);
        loadingText.SetText(loadingText.GetParsedText()+".");
        yield return new WaitForSeconds(0.2f);
        loadingText.SetText(loadingText.GetParsedText() + ".");
        yield return new WaitForSeconds(0.2f);
        loadingText.SetText(loadingText.GetParsedText() + ".");
    }
}
