using GWM = GameWorldManager;

public class PS_SunStroke : PlayerStat
{
    #region LikeSingleton
    private PS_SunStroke() : base() { }
    public static PS_SunStroke Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_SunStroke();
        }
        return null;
    }
    #endregion



    protected override void OnStart()
    {

    }

    protected override void CheckValue()
    {

    }

    protected override void SetValue()
    {

    }
}