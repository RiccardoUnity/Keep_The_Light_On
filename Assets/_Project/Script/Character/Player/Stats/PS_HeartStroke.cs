using System;
using GWM = GameWorldManager;

public class PS_HeartStroke : PlayerStat
{
    #region LikeSingleton
    private PS_HeartStroke() : base() { }
    public static Func<bool> Instance(int key, out PS_HeartStroke heartStroke, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            heartStroke = new PS_HeartStroke();
            _debug = debug;
            return heartStroke.MyAwake;
        }
        heartStroke = null;
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