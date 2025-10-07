using System;

public static partial class S_SaveSystem
{
    [Serializable]
    private class Save_GameWorldManager
    {
        //Time
        public int currentSecondDay;
        public int currentDay;

        public void Save()
        {
            currentSecondDay = GameWorldManager.Instance.TimeManager.currentSecondDay;
            currentDay = GameWorldManager.Instance.TimeManager.currentDay;
        }

        public void Load()
        {
            GameWorldManager.Instance.TimeManager.StartTime(currentSecondDay, currentDay);
        }
    }
}