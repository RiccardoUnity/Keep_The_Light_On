using UnityEngine;

[CreateAssetMenu(fileName = (nameof(SO_ItemCraft)), menuName = "Data/" + (nameof(SO_ItemCraft)))]
public class SO_ItemCraft : ScriptableObject
{
    public SO_Item[] SOItemToCraft { get => _soItemToCraft; }
    [SerializeField] private SO_Item[] _soItemToCraft;
    public SO_Item[] SOItemResources { get => _soItemResources; }
    [SerializeField] private SO_Item[] _soItemResources;

    public bool UseCampfire { get => _useCampfire; }
    [SerializeField] private bool _useCampfire;

    public float RealSecondsToCraft { get => _realMinutesToCraft * 60f; }
    [SerializeField] private float _realMinutesToCraft = 10;
}
