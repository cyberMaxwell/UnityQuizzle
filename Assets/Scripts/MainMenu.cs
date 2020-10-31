using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Text livesText;

    private void Start()
    {
        livesText.text = PlayerPrefs.GetInt("livesCount", 3).ToString();

    }

   

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

}
