using UnityEngine;
using System;
using Save;

[Serializable]
public class Save_GameScene
{
    //Time
    public int currentSecondDay;
    public int currentDay;

    public void Save()
    {
        currentSecondDay = S_TimeManager.currentSecondDay;
        currentDay = S_TimeManager.currentDay;
    }

    public void Load()
    {
        S_TimeManager.StartTime(currentSecondDay, currentDay);
    }
}