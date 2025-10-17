using GWM = GameWorldManager;

public class PS_Hunger : PlayerStat
{
    #region LikeSingleton
    private PS_Hunger() : base() { }
    public static PS_Hunger Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_Hunger();
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