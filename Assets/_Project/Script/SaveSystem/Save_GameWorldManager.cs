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
            currentSecondDay = GameWorldManager.Instance.timeGame.currentSecondDay;
            currentDay = GameWorldManager.Instance.timeGame.currentDay;
        }

        public void Load()
        {
            GameWorldManager.Instance.timeGame.StartTime(currentSecondDay, currentDay);
        }
    }
}