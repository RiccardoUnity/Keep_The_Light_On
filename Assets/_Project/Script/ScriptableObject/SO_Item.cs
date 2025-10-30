using UnityEngine;

[CreateAssetMenu(fileName = (nameof(SO_Item)), menuName = "Data/" + (nameof(SO_Item)))]

public class SO_Item : ScriptableObject
{
    [System.Serializable]
    private class Modifier
    {
        [Range(0f, 1f)][SerializeField] private float _lessOrEqualToCondition = 1f;
        [SerializeField] private PlayerStats _stat = PlayerStats.Life;
        [SerializeField] private bool _isIncrease;
        [SerializeField] private int _minutesRealTimeActive = 6;
        [Range(0f, 1f)][SerializeField] private float _value = 0.1f;
        private PlayerStat _playerStat;

        private void SetPlayerStat(PlayerManager playerManager)
        {
            switch (_stat)
            {
                case PlayerStats.Life:
                    _playerStat = playerManager.Life;
                    break;
                case PlayerStats.Enduro:
                    _playerStat = playerManager.Endurance;
                    break;
                case PlayerStats.Rest:
                    _playerStat = playerManager.Rest;
                    break;
                case PlayerStats.Hunger:
                    _playerStat = playerManager.Hunger;
                    break;
                case PlayerStats.Thirst:
                    _playerStat = playerManager.Thirst;
                    break;
                case PlayerStats.Stamina:
                    _playerStat = playerManager.Stamina;
                    break;
                case PlayerStats.SunStroke:
                    _playerStat = playerManager.SunStroke;
                    break;
                case PlayerStats.StomachAche:
                    _playerStat = playerManager.StomachAche;
                    break;
                case PlayerStats.HeartAche:
                    _playerStat = playerManager.HeartAche;
                    break;
            }
        }

        public void Apply(PlayerManager playerManager, float realSecondToGameSecond, float condition)
        {
            if (condition <= _lessOrEqualToCondition)
            {
                SetPlayerStat(playerManager);
                float secondsGameTime = ((_minutesRealTimeActive * 60) / realSecondToGameSecond);
                float value = _value / secondsGameTime;

                _playerStat.AddModifier(value, _isIncrease, secondsGameTime);
            }
        }

    }

    public string Name { get => _name; }
    [SerializeField] private string _name;

    public string Description { get => _description; }
    [SerializeField] [TextArea(2, 4)] private string _description;

    public Sprite Icon { get => _icon; }
    [SerializeField] private Sprite _icon;
    public Color Color { get => _color; }
    [SerializeField] private Color _color = Color.white;

    public Prefab_Item Prefab { get => _prefab; }
    [SerializeField] public Prefab_Item _prefab;

    public float HarvestTime { get => _harvestTime; }
    [SerializeField] private float _harvestTime = 0.5f;

    public float Weight { get => _weight; }
    [SerializeField] private float _weight = 0.1f;

    public ItemType ItemType { get => _itemType; }
    [SerializeField] private ItemType _itemType;

    public ItemTool ItemTool { get => _itemTool; }
    [SerializeField] private ItemTool _itemTool;

    public ItemCampfire ItemCampfire { get => _itemCampfire; }
    [SerializeField] private ItemCampfire _itemCampfire;

    public float MinutesFuel { get => _minutesFuel; }
    [SerializeField] private float _minutesFuel;

    public bool CanUse { get => _canUse; }
    [SerializeField] private bool _canUse;
    public bool RemoveAfterUse { get => _removeAfterUse; }
    [SerializeField] private bool _removeAfterUse;
    public ItemTool ItemToolCanUse { get => _itemToolCanUse; }
    [SerializeField] private ItemTool _itemToolCanUse;
    [SerializeField] private Modifier[] _modifiers;
    public SO_Item[] SOItemUse { get => _soItemUse; }
    [SerializeField] private SO_Item[] _soItemUse;

    public bool Use(PlayerManager playerManager, GameWorldManager gwm, float condition, bool isInInventory)
    {
        if (_canUse)
        {
            if (ItemType == ItemType.Dress)
            {
                Dress();
                return true;
            }
            else if (ItemType == ItemType.Tool && ItemTool == ItemTool.SleepingBag)
            {
                Sleep(gwm);
                return true;
            }
            else
            {
                if (ItemToolCanUse == ItemTool.None || (ItemToolCanUse != ItemTool.None && playerManager.PlayerInventory.HasToolInInventory(ItemToolCanUse)))
                {
                    foreach (Modifier mod in _modifiers)
                    {
                        mod.Apply(playerManager, gwm.TimeManager.RealSecondToGameSecond, condition);
                    }

                    Data_Item dataItem;
                    foreach (SO_Item item in _soItemUse)
                    {
                        dataItem = gwm.PoolManager.RemoveDataItemFromPool(item, condition, ItemState.New);
                        playerManager.PlayerInventory.AddItemInventory(dataItem);
                    }

                    if (!isInInventory)
                    {
                        playerManager.RemoveItem();
                    }

                    return true;
                }
            }
        }
        return false;
    }

    private void Dress()
    {

    }

    private void Sleep(GameWorldManager gwm)
    {
        gwm.UIInventory.gameObject.SetActive(true);
        gwm.UIInventory.OpenUIBed();
        gwm.UIStats.gameObject.SetActive(false);
    }
}