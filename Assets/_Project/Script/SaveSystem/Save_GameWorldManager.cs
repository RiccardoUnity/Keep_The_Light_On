using System;

[Serializable]
public class Save_GameWorldManager
{
    //Time
    public int currentSecondDay;
    public int currentDay;

    public void Save()
    {
        currentSecondDay = GameWorldManager.Instance.timeGame.currentSecondDay;
        currentDay = GameWorldManager.Instance.timeGame.currentDay;
    }

    public void Load()
    {
        GameWorldManager.Instance.timeGame.StartTime(currentSecondDay, currentDay);
    }
}