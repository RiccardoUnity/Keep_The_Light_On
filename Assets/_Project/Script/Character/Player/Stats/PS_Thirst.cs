using GWM = GameWorldManager;

public class PS_Thirst : PlayerStat
{
    #region LikeSingleton
    private PS_Thirst() : base() { }
    public static PS_Thirst Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_Thirst();
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