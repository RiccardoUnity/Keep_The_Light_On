using System;
using GWM = GameWorldManager;

public class PS_Thirst : PlayerStat
{
    #region LikeSingleton
    private PS_Thirst() : base() { }
    public static Func<bool> Instance(int key, out PS_Thirst thirst, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            thirst = new PS_Thirst();
            _debug = debug;
            return thirst.MyAwake;
        }
        thirst = null;
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