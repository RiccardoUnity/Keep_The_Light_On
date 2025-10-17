using GWM = GameWorldManager;

public class PS_Life : PlayerStat
{
    #region LikeSingleton
    private PS_Life() : base() { }
    public static PS_Life Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_Life();
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