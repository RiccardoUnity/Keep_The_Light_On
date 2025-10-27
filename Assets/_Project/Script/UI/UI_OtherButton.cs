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

    public int Index { get => _index; }
    private int _index;
    private string[] _texts;

    public void SetUp(string[] texts, int startIndex)
    {
        _texts = texts;
        SetIndex(startIndex);
    }

    public void ButtonLess() => Select(false);
    public void ButtonGreat() => Select(true);

    private void Select(bool isNext)
    {
        if (isNext)
        {
            if (_index == _texts.Length - 1)
            {
                _index = 0;
            }
            else
            {
                ++_index;
            }
        }
        else
        {
            if (_index == 0)
            {
                _index = _texts.Length - 1;
            }
            else
            {
                --_index;
            }
        }

        Text.text = _texts[_index];
    }

    public void SetIndex(int index)
    {
        if (index < _texts.Length)
        {
            _index = index;
        }
        else
        {
            _index = 0;
        }
        Text.text = _texts[_index];
    }
}
