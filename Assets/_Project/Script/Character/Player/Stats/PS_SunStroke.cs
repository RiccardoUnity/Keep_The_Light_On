using System;
using GWM = GameWorldManager;

public class PS_SunStroke : PlayerStat
{
    #region LikeSingleton
    private PS_SunStroke() : base() { }
    public static Func<bool> Instance(int key, out PS_SunStroke sunStroke, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            sunStroke = new PS_SunStroke();
            _debug = debug;
            return sunStroke.MyAwake;
        }
        sunStroke = null;
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