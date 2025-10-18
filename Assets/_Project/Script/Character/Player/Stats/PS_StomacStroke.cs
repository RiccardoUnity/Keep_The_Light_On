using System;
using GWM = GameWorldManager;

public class PS_StomacStroke : PlayerStat
{
    #region LikeSingleton
    private PS_StomacStroke() : base() { }
    public static Func<bool> Instance(int key, out PS_StomacStroke stomacStroke, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            stomacStroke = new PS_StomacStroke();
            _debug = debug;
            return stomacStroke.MyAwake;
        }
        stomacStroke = null;
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