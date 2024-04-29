using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pauseCountDowntext;

    #region Animation Event

    public void Three()
    {
        pauseCountDowntext.text = "3";
    }
    public void Two()
    {
        pauseCountDowntext.text = "2";
    }
    public void One()
    {
        pauseCountDowntext.text = "1";
    }
    public void DisableText()
    {
        Time.timeScale = 1.0f;
        pauseCountDowntext.gameObject.SetActive(false);
    }
    #endregion
}
