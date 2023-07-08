using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager Instance { get; private set; }
    [Header("UI Menu")] 
    public RectTransform headerMenu;
    public RectTransform pauseMenu;
    public RectTransform endMenu;
    [Header("End Menu Text Fields")] 
    public Text gameOverText;
    public Text rankText;
    public Text endScoreText;
    [Space(20)]
    public Image timerFill;
    [Header("Texts")]
    public Text remainingTimeText;
    public Text playerScoreText;
    
    private void Awake()
    {
        // Implement Singleton
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerScoreText.text = GameManager.Instance.playerController.Score.ToString();
    }

    public void OnPause()
    {
        headerMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
    }

    public void OnResume()
    {
        pauseMenu.gameObject.SetActive(false);
        headerMenu.gameObject.SetActive(true);
    }

    private void PlayerWin()
    {
        gameOverText.text = "You Win!";
        rankText.text = "You're #0";
        endScoreText.text = $"Score: {GameManager.Instance.playerController.Score.ToString()}";
    }
    
    private void PlayerLose()
    {
        gameOverText.text = "You Lose!";
        rankText.text = "You're #0";
        endScoreText.text = $"Score: {GameManager.Instance.playerController.Score.ToString()}";
    }
    
    public void OnFinish(bool isWin)
    {
        // Set End Menu texts
        if (isWin)
        {
            PlayerWin();
        }
        else
        {
            PlayerLose();
            
        }
        // Show Scoreboard and End Menu
        headerMenu.gameObject.SetActive(false);
        endMenu.gameObject.SetActive(true);
    }
}
