using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerController playerController;
    public List<Wrestler> allWrestlers;
    public float gameArenaRadius;
    private Timer _timer;
    
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
            allWrestlers.Add(playerController);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Prevents timescale delay after the restart game.
        Time.timeScale = 1f;
        _timer = GetComponent<Timer>();
        _timer.SetTimerOnUI(_timer.gameDuration);
        StartCoroutine(StartGameInSeconds(3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartGameInSeconds(int second)
    {
        while (second >= 0)
        {
            UserInterfaceManager.Instance.countDownText.text = second.ToString();
            second--;
            yield return new WaitForSeconds(1f);
        }
        UserInterfaceManager.Instance.countDownText.enabled = false;
        _timer.enabled = true;
        foreach (var wrestler in allWrestlers)
        {
            wrestler.enabled = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void PauseGame()
    {
        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            UserInterfaceManager.Instance.OnPause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UserInterfaceManager.Instance.OnResume();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    
    public void FinishGame()
    {
        UserInterfaceManager.Instance.OnFinish(playerController.IsWin);
        Time.timeScale = 0f;
    }

    public void CheckEnemyCount()
    {
        if (allWrestlers.Count == 1)
        {
            FinishGame();
        }
    }
}
