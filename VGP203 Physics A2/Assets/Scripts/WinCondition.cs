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
        //currentTime = 10;
        carControllers = FindAnyObjectByType<CarControllers>();

        currentTime = 390;
    }

    private void HandlePlayerSpawned()
    {
        startTimer();
    }
  
    private void Update()
    {
        if (timerActive)
        {
            if (currentTime > 0) currentTime = currentTime - Time.deltaTime;

            if (currentTime <= 0)
            {
                stopTimer();

                carControllers.loseScreen.gameObject.SetActive(true);
                carControllers.inCar = false;
            }
        }
        //TimeSpan time = TimeSpan.FromSeconds(currentTime);
        TimeSpan time = TimeSpan.FromSeconds(Mathf.Max(currentTime, 0));
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





