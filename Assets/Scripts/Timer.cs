using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    public int gameDuration;
    private int _remainingTime;
    
    // Start is called before the first frame update
    void Start()
    {
        BeginTimer(gameDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimerOnUI(int remainingTime)
    {
        UserInterfaceManager.Instance.timerFill.fillAmount = Mathf.InverseLerp(0, gameDuration, remainingTime);
        UserInterfaceManager.Instance.remainingTimeText.text = $"{remainingTime / 60:0}:{remainingTime % 60:00}";
    }
    
    private void BeginTimer(int second)
    {
        _remainingTime = second;
        StartCoroutine(UpdateTimer());
    }
    
    private IEnumerator UpdateTimer()
    {
        while (_remainingTime >= 0)
        {
            SetTimerOnUI(_remainingTime);
            _remainingTime--;
            yield return new WaitForSeconds(1f);
        }
        // End
        OnEnd();
    }

    private void OnEnd()
    {
        GameManager.Instance.FinishGame();
    }
}
