using GWM = GameWorldManager;

public class PS_StomacStroke : PlayerStat
{
    #region LikeSingleton
    private PS_StomacStroke() : base() { }
    public static PS_StomacStroke Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_StomacStroke();
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