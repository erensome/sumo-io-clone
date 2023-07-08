using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerController playerController;
    
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
        
    }

    public void PauseGame()
    {
        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            UserInterfaceManager.Instance.OnPause();
            print("paused");
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UserInterfaceManager.Instance.OnResume();
        print("resume");
    }

    public void FinishGame()
    {
        // First check for is player alive
        UserInterfaceManager.Instance.OnFinish(playerController.IsWin);
        Time.timeScale = 0f;
    }
}
