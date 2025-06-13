using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    bool timerActive = false;
    float currentTime;
    public TMP_Text currentTimeText;
    public Transform playerTransform;
    CarControllers carControllers;

    private void Start()
    {
        currentTime = 120;
    }

    private void HandlePlayerSpawned()
    {
        startTimer();
    }
  
    private void Update()
    {
        if (timerActive)
        {
            currentTime = currentTime - Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString("D2");
        //currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    public string getTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        return currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString("D2");
    }
    public void startTimer()
    {
        timerActive = true;
    }
    public void stopTimer()
    {
        timerActive = false;
    }
}





