using System;
using GWM = GameWorldManager;

public class PS_Stamina : PlayerStat
{
    #region LikeSingleton
    private PS_Stamina() : base() { }
    public static Func<bool> Instance(int key, out PS_Stamina stamina, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            stamina = new PS_Stamina();
            _debug = debug;
            return stamina.MyAwake;
        }
        stamina = null;
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