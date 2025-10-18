using System;

public class PS_Hunger : PlayerStat
{
    #region LikeSingleton
    private PS_Hunger() : base() { }
    public static Func<bool> Instance(int key, out PS_Hunger hunger, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            hunger = new PS_Hunger();
            _debug = debug;
            return hunger.MyAwake;
        }
        hunger = null;
        return null;
    }
    #endregion



    protected override void OnAwake()
    {

    }

    protected override void CheckValue()
    {

    }

    protected override void SetValue(int secondsDelay)
    {

    }
}