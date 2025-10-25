using TMPro;
using UnityEngine;

public class UI_OtherButton : MonoBehaviour
{
    public UI_Button Less { get => _less; }
    [SerializeField] private UI_Button _less;
    public TMP_Text Text { get => _text; }
    [SerializeField] private TMP_Text _text;
    public UI_Button Great { get => _great; }
    [SerializeField] private UI_Button _great;
}
