using GWM = GameWorldManager;

public class PS_Stamina : PlayerStat
{
    #region LikeSingleton
    private PS_Stamina() : base() { }
    public static PS_Stamina Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_Stamina();
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