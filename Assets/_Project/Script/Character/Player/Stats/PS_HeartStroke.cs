using GWM = GameWorldManager;

public class PS_HeartStroke : PlayerStat
{
    #region LikeSingleton
    private PS_HeartStroke() : base() { }
    public static PS_HeartStroke Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_HeartStroke();
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